namespace Xrm.Json.Serialization.Tests
{
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using Xunit;

    public class OptionSetValueCollectionConverterTests
    {
        #region Public Methods

        [Fact]
        public void OptionSetValueCollection_Empty_Can_Serialize()
        {
            // Arrange
            var value = new OptionSetValueCollection();
            var expected = "{\"_options\":[]}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new OptionSetValueCollectionConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OptionSetValueCollection_Empty_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_options\":[]}";

            // Act
            var actual = JsonConvert.DeserializeObject<OptionSetValueCollection>(json, new OptionSetValueCollectionConverter());

            // Assert
            Assert.NotNull(actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void OptionSetValueCollection_Single_Can_Serialize()
        {
            // Arrange
            var value = new OptionSetValueCollection { new OptionSetValue(1) };
            var expected = "{\"_options\":[1]}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new OptionSetValueCollectionConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OptionSetValueCollection_Single_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_options\":[1]}";

            // Act
            var actual = JsonConvert.DeserializeObject<OptionSetValueCollection>(json, new OptionSetValueCollectionConverter());

            // Assert
            Assert.NotNull(actual);
            Assert.Single(actual);
            Assert.Equal(1, actual[0].Value);
        }

        [Fact]
        public void OptionSetValueCollection_Multiple_Can_Serialize()
        {
            // Arrange
            var value = new OptionSetValueCollection 
            { 
                new OptionSetValue(1),
                new OptionSetValue(2),
                new OptionSetValue(3)
            };
            var expected = "{\"_options\":[1,2,3]}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new OptionSetValueCollectionConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OptionSetValueCollection_Multiple_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_options\":[1,2,3]}";

            // Act
            var actual = JsonConvert.DeserializeObject<OptionSetValueCollection>(json, new OptionSetValueCollectionConverter());

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(3, actual.Count);
            Assert.Equal(1, actual[0].Value);
            Assert.Equal(2, actual[1].Value);
            Assert.Equal(3, actual[2].Value);
        }

        [Fact]
        public void OptionSetValueCollection_LargeNumbers_Can_Serialize()
        {
            // Arrange
            var value = new OptionSetValueCollection 
            { 
                new OptionSetValue(100000000),
                new OptionSetValue(200000000),
                new OptionSetValue(300000000)
            };
            var expected = "{\"_options\":[100000000,200000000,300000000]}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new OptionSetValueCollectionConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OptionSetValueCollection_LargeNumbers_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_options\":[100000000,200000000,300000000]}";

            // Act
            var actual = JsonConvert.DeserializeObject<OptionSetValueCollection>(json, new OptionSetValueCollectionConverter());

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(3, actual.Count);
            Assert.Equal(100000000, actual[0].Value);
            Assert.Equal(200000000, actual[1].Value);
            Assert.Equal(300000000, actual[2].Value);
        }

        #endregion Public Methods
    }
}
