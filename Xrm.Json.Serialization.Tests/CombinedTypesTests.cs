namespace Xrm.Json.Serialization.Tests
{
    using System;
    using Xrm.Json.Serialization;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using Xunit;

    public class CombinedTypesTests
    {
        #region Public Methods

        [Fact]
        public void Entity_Can_Deserialize_With_Mixed_Types()
        {
            // Arrange
            var name = "test";
            var id = Guid.NewGuid();
            var someGuid = Guid.NewGuid();
            var refEntName = "refEnt";
            var refEntId = Guid.NewGuid();

            var expected = new Entity(name, id);
            expected.Attributes.Add("someString", "testString");
            expected.Attributes.Add("someGuid", someGuid);
            expected.Attributes.Add(refEntName, new EntityReference(refEntName, refEntId));
            expected.Attributes.Add("attribute1", new OptionSetValue(1));

            var value = "{" +
                $"\"_reference\":\"{name}:{id.ToString()}\"," +
                "\"someString\":\"testString\"," +
                $"\"someGuid\":{{\"_id\":\"{someGuid.ToString()}\"}}," +
                $"\"{refEntName}\":{{\"_reference\":\"{refEntName}:{refEntId}\"}}," +
                "\"attribute1\":{\"_option\":1}" +
                "}";

            // Act
            var actual = JsonConvert.DeserializeObject<Entity>(value, new EntityConverter());

            // Assert
            Assert.Equal(expected.LogicalName, actual.LogicalName);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Attributes["someString"], actual.Attributes["someString"]);
            Assert.Equal(expected.Attributes["someGuid"], actual.Attributes["someGuid"]);
            Assert.Equal((expected.Attributes[refEntName] as EntityReference).Id, (expected.Attributes[refEntName] as EntityReference).Id);
            Assert.Equal((expected.Attributes[refEntName] as EntityReference).LogicalName, (expected.Attributes[refEntName] as EntityReference).LogicalName);
            Assert.Equal((expected.Attributes["attribute1"] as OptionSetValue).Value, (actual.Attributes["attribute1"] as OptionSetValue).Value);
        }

        [Fact]
        public void Entity_Can_Serialize_With_Mixed_Types()
        {
            // Arrange
            var name = "test";
            var id = Guid.NewGuid();
            var value = new Entity(name, id);

            var someGuid = Guid.NewGuid();
            var refEntName = "refEnt";
            var refEntId = Guid.NewGuid();
            value.Attributes.Add("someString", "testString");
            value.Attributes.Add("someGuid", someGuid);
            value.Attributes.Add(refEntName, new EntityReference(refEntName, refEntId));
            value.Attributes.Add("attribute1", new OptionSetValue(1));

            var expected = "{" +
                $"\"_reference\":\"{name}:{id.ToString()}\"," +
                "\"someString\":\"testString\"," +
                $"\"someGuid\":{{\"_id\":\"{someGuid.ToString()}\"}}," +
                $"\"{refEntName}\":{{\"_reference\":\"{refEntName}:{refEntId}\"}}," +
                "\"attribute1\":{\"_option\":1}" +
                "}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new EntityConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion Public Methods
    }
}