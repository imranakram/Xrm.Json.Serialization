namespace Xrm.Json.Serialization.Tests
{
    using System;
    using Xrm.Json.Serialization;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using Xunit;

    public class EntityConverterTests
    {
        #region Public Methods

        [Fact]
        public void Entity_Can_Deserialize()
        {
            // Arrange
            var name = "test";
            var id = Guid.NewGuid();
            var expected = new Entity(name, id);
            expected.Attributes.Add("attribute1", new OptionSetValue(1));
            expected.Attributes.Add("attribute2", 2);
            expected.Attributes.Add("attribute3", 13.37d);
            var value = $"{{\"_reference\":\"{name}:{id.ToString()}\",\"attribute1\":{{\"_option\":1}},\"attribute2\":2,\"attribute3\":13.37}}"; ;

            // Act
            var actual = JsonConvert.DeserializeObject<Entity>(value, new EntityConverter());

            // Assert
            Assert.Equal(expected.LogicalName, actual.LogicalName);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal((expected.Attributes["attribute1"] as OptionSetValue).Value, (actual.Attributes["attribute1"] as OptionSetValue).Value);
            Assert.Equal((int)expected.Attributes["attribute2"], (int)actual.Attributes["attribute2"]);
            Assert.Equal((double)expected.Attributes["attribute3"], (double)actual.Attributes["attribute3"]);
        }

        [Fact]
        public void Entity_Can_Remove_Empty_Attribute_During_Serialize()
        {
            // Arrange
            var name = "test";
            var id = Guid.NewGuid();
            var value = new Entity(name, id);
            value.Attributes.Add("attribute1", null);
            var expected = $"{{\"_reference\":\"{name}:{id}\"}}"; ;

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new EntityConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Entity_Can_Report_Incorrect_Attribute_During_Deserialize()
        {
            // Arrange
            var name = "test";
            var id = Guid.NewGuid();
            var value = $"{{\"_reference\":\"{name}:{id}\",\"attribute1\":}}"; // Intentional error: value of `attribute1` is missing!

            // Act
            var ex = Assert.Throws<JsonException>(() => JsonConvert.DeserializeObject<Entity>(value, new EntityConverter()));

            // Assert
            Assert.Contains("attribute1", ex.Message);
        }

        [Fact]
        public void Entity_Can_Serialize()
        {
            // Arrange
            var name = "test";
            var id = Guid.NewGuid();
            var value = new Entity(name, id);
            value.Attributes.Add("attribute1", new OptionSetValue(1));
            value.Attributes.Add("attribute2", 2);
            value.Attributes.Add("attribute3", 13.37d);
            value.Attributes.Add("attribute4", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            var expected = $"{{\"_reference\":\"{name}:{id.ToString()}\",\"attribute1\":{{\"_option\":1}},\"attribute2\":2,\"attribute3\":13.37,\"attribute4\":\"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\"}}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new EntityConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion Public Methods
    }
}