namespace Xrm.Json.Serialization.Tests
{
    using System;
    using Xrm.Json.Serialization;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using Xunit;

    public class EntityCollectionConverterTests
    {
        #region Public Methods

        [Fact]
        public void EntityCollection_Can_Deserialize()
        {
            // Arrange
            var name = "test";
            var id = Guid.NewGuid();
            var entity = new Entity(name, id);
            entity.Attributes.Add("attribute1", new OptionSetValue(1));
            var expected = new EntityCollection();
            expected.Entities.Add(entity);
            var value = $"[{{\"_reference\":\"{name}:{id.ToString()}\",\"attribute1\":{{\"_option\":1}}}}]"; ;

            // Act
            var actual = JsonConvert.DeserializeObject<EntityCollection>(value, new EntityCollectionConverter());

            // Assert
            Assert.Equal(expected.Entities.Count, actual.Entities.Count);
            //Assert.Equal(expected.Id, actual.Id);
            //Assert.Equal((expected.Attributes["attribute1"] as OptionSetValue).Value, (actual.Attributes["attribute1"] as OptionSetValue).Value);
        }

        [Fact]
        public void EntityCollection_Can_Serialize()
        {
            // Arrange
            var name = "test";
            var id = Guid.NewGuid();
            var entity = new Entity(name, id);
            entity.Attributes.Add("attribute1", new OptionSetValue(1));
            var value = new EntityCollection();
            value.Entities.Add(entity);

            var expected = $"[{{\"_reference\":\"{name}:{id.ToString()}\",\"attribute1\":{{\"_option\":1}}}}]";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new EntityCollectionConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion Public Methods
    }
}