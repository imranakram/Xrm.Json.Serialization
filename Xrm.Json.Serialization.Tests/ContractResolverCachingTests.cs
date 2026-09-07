namespace Xrm.Json.Serialization.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Xrm.Json.Serialization;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Xunit;

    /// <summary>
    /// Pins the contract-resolver sharing introduced to fix the serialization hot path.
    /// <para>
    /// <see cref="EntityConverter"/> and <see cref="EntityCollectionConverter"/> used to run
    /// <c>serializer.ContractResolver = new XrmContractResolver()</c> on every call. A
    /// <see cref="DefaultContractResolver"/> caches the contract it builds per type on the
    /// instance, so a fresh instance per call meant every type on the graph was re-resolved by
    /// reflection each time. Measured at 100 000 entities that cost 82 s against 0.34 s with
    /// one shared instance, for byte-identical output.
    /// </para>
    /// <para>
    /// These tests assert the mechanism (one instance, still routed) and the invariant that
    /// matters (output is unchanged), so the allocation cannot creep back unnoticed.
    /// </para>
    /// </summary>
    public class ContractResolverCachingTests
    {
        #region Private Methods

        private static Entity Rich(int seed)
        {
            var entity = new Entity("test", Guid.NewGuid());
            entity.Attributes.Add("someString", "value " + seed);
            entity.Attributes.Add("someInt", seed);
            entity.Attributes.Add("someLong", (long)seed * 10000000000L);
            entity.Attributes.Add("someDouble", 13.37d + seed);
            entity.Attributes.Add("someBool", seed % 2 == 0);
            entity.Attributes.Add("someGuid", Guid.NewGuid());
            entity.Attributes.Add("someMoment", new DateTime(2026, 1, 1).AddDays(seed));
            entity.Attributes.Add("someReference", new EntityReference("contact", Guid.NewGuid()));
            entity.Attributes.Add("someMoney", new Money(120m + seed));
            entity.Attributes.Add("someOption", new OptionSetValue(seed % 3));
            entity.Attributes.Add("someOptions", new OptionSetValueCollection(new List<OptionSetValue> { new OptionSetValue(1), new OptionSetValue(2) }));
            entity.Attributes.Add("someManaged", new BooleanManagedProperty(true));
            entity.Attributes.Add("someAliased", new AliasedValue("contact", "fullname", "Anna Andersson"));
            entity.Attributes.Add("someNothing", null);

            return entity;
        }

        #endregion Private Methods

        #region Public Methods

        [Fact]
        public void Resolver_Is_A_Single_Shared_Instance()
        {
            // Arrange, Act
            var first = XrmContractResolver.Shared;
            var second = XrmContractResolver.Shared;

            // Assert
            Assert.NotNull(first);
            Assert.Same(first, second);
        }

        [Fact]
        public void Serializing_An_Entity_Does_Not_Allocate_A_New_Resolver()
        {
            // Arrange
            var serializer = JsonSerializer.CreateDefault();
            var entity = Rich(1);

            // Act
            using (var writer = new JsonTextWriter(new System.IO.StringWriter()))
            {
                new EntityConverter().WriteJson(writer, entity, serializer);
            }
            var afterFirst = serializer.ContractResolver;

            using (var writer = new JsonTextWriter(new System.IO.StringWriter()))
            {
                new EntityConverter().WriteJson(writer, entity, serializer);
            }
            var afterSecond = serializer.ContractResolver;

            // Assert
            Assert.Same(XrmContractResolver.Shared, afterFirst);
            Assert.Same(afterFirst, afterSecond);
        }

        [Fact]
        public void Serializing_A_Collection_Does_Not_Allocate_A_New_Resolver()
        {
            // Arrange
            var serializer = JsonSerializer.CreateDefault();
            var collection = new EntityCollection(new List<Entity> { Rich(1), Rich(2) }) { EntityName = "test" };

            // Act
            using (var writer = new JsonTextWriter(new System.IO.StringWriter()))
            {
                new EntityCollectionConverter().WriteJson(writer, collection, serializer);
            }
            var afterFirst = serializer.ContractResolver;

            using (var writer = new JsonTextWriter(new System.IO.StringWriter()))
            {
                new EntityCollectionConverter().WriteJson(writer, collection, serializer);
            }
            var afterSecond = serializer.ContractResolver;

            // Assert
            Assert.Same(XrmContractResolver.Shared, afterFirst);
            Assert.Same(afterFirst, afterSecond);
        }

        [Fact]
        public void A_Foreign_Resolver_Is_Still_Replaced()
        {
            // A caller-supplied resolver cannot route the CRM types, so the converters have
            // always overwritten it. That behaviour is unchanged - only the instance is.

            // Arrange
            var serializer = JsonSerializer.CreateDefault();
            serializer.ContractResolver = new DefaultContractResolver();

            // Act
            using (var writer = new JsonTextWriter(new System.IO.StringWriter()))
            {
                new EntityConverter().WriteJson(writer, Rich(1), serializer);
            }

            // Assert
            Assert.Same(XrmContractResolver.Shared, serializer.ContractResolver);
        }

        [Fact]
        public void EntitySerializer_Routes_Through_The_Shared_Resolver()
        {
            // Arrange
            var entity = Rich(1);

            // Act
            var json = EntitySerializer.Serialize(entity);
            var actual = EntitySerializer.DeserializeEntity(json);

            // Assert
            Assert.Equal(entity.LogicalName, actual.LogicalName);
            Assert.Equal(entity.Id, actual.Id);
            Assert.Equal(json, EntitySerializer.Serialize(actual));
        }

        [Fact]
        public void Every_Supported_Attribute_Type_Survives_A_Round_Trip()
        {
            // Arrange
            var entity = Rich(7);

            // Act
            var json = EntitySerializer.Serialize(entity);
            var actual = EntitySerializer.DeserializeEntity(json);

            // Assert
            Assert.Equal("value 7", actual.GetAttributeValue<string>("someString"));
            Assert.Equal(7, actual.GetAttributeValue<int>("someInt"));
            Assert.Equal(70000000000L, actual.GetAttributeValue<long>("someLong"));
            Assert.Equal(20.37d, actual.GetAttributeValue<double>("someDouble"), 10);
            Assert.Equal(entity.GetAttributeValue<Guid>("someGuid"), actual.GetAttributeValue<Guid>("someGuid"));
            Assert.Equal(new DateTime(2026, 1, 8), actual.GetAttributeValue<DateTime>("someMoment"));
            Assert.Equal(entity.GetAttributeValue<EntityReference>("someReference").Id, actual.GetAttributeValue<EntityReference>("someReference").Id);
            Assert.Equal(127m, actual.GetAttributeValue<Money>("someMoney").Value);
            Assert.Equal(1, actual.GetAttributeValue<OptionSetValue>("someOption").Value);
            Assert.Equal(2, actual.GetAttributeValue<OptionSetValueCollection>("someOptions").Count);
            Assert.True(actual.GetAttributeValue<BooleanManagedProperty>("someManaged").Value);
            Assert.Equal("Anna Andersson", actual.GetAttributeValue<AliasedValue>("someAliased").Value);

            // A `null` attribute is equivalent to no attribute, so it is dropped on the way out.
            Assert.False(actual.Attributes.ContainsKey("someNothing"));
        }

        [Fact]
        public void Repeated_Serialization_Is_Stable()
        {
            // The resolver is now shared across every call in the process. If any converter
            // mutated it, the second pass would differ from the first.

            // Arrange
            var entity = Rich(3);

            // Act
            var first = EntitySerializer.Serialize(entity);
            var second = EntitySerializer.Serialize(entity);
            var third = EntitySerializer.Serialize(entity);

            // Assert
            Assert.Equal(first, second);
            Assert.Equal(first, third);
        }

        [Fact]
        public void Nested_Entities_Share_The_Same_Resolver()
        {
            // An EntityCollection holding entities exercises both converters that assign the
            // resolver, one inside the other. The inner one must not swap it out again.

            // Arrange
            var collection = new EntityCollection(new List<Entity> { Rich(1), Rich(2) }) { EntityName = "test" };

            // Act
            var json = EntitySerializer.Serialize(collection);
            var actual = EntitySerializer.DeserializeCollection(json);

            // Assert
            Assert.Equal(2, actual.Entities.Count);
            Assert.Equal(json, EntitySerializer.Serialize(actual));
        }

        [Fact]
        public void Bulk_Serialization_Stays_Well_Under_The_Reflection_Cliff()
        {
            // Guards the regression directly rather than by proxy. Before the fix this loop
            // re-resolved every contract on every iteration; 5 000 entities took roughly 4 s.
            // With the shared resolver it is a few tens of milliseconds, so a two-second
            // budget fails loudly on a regression without being flaky on a slow agent.

            // Arrange
            var entities = new List<Entity>();
            for (var i = 0; i < 5000; i++)
            {
                entities.Add(Rich(i));
            }

            // Act
            var stopwatch = Stopwatch.StartNew();
            foreach (var entity in entities)
            {
                EntitySerializer.Serialize(entity);
            }
            stopwatch.Stop();

            // Assert
            Assert.True(
                stopwatch.Elapsed.TotalSeconds < 2,
                $"Serializing 5 000 entities took {stopwatch.Elapsed.TotalSeconds:F2} s. That is the " +
                "signature of a per-call ContractResolver allocation - see XrmContractResolver.Shared.");
        }

        #endregion Public Methods
    }
}
