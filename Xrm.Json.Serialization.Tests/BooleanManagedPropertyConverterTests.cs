namespace Xrm.Json.Serialization.Tests
{
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using Xunit;

    public class BooleanManagedPropertyConverterTests
    {
        #region Public Methods

        [Fact]
        public void BooleanManagedProperty_True_CanChange_Can_Serialize()
        {
            // Arrange
            var value = new BooleanManagedProperty(true) { CanBeChanged = true };
            var expected = "{\"_boolmanaged\":\"True|True\"}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new BooleanManagedPropertyConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BooleanManagedProperty_True_CanChange_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_boolmanaged\":\"True|True\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<BooleanManagedProperty>(json, new BooleanManagedPropertyConverter());

            // Assert
            Assert.True(actual.Value);
            Assert.True(actual.CanBeChanged);
        }

        [Fact]
        public void BooleanManagedProperty_True_CannotChange_Can_Serialize()
        {
            // Arrange
            var value = new BooleanManagedProperty(true) { CanBeChanged = false };
            var expected = "{\"_boolmanaged\":\"True|False\"}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new BooleanManagedPropertyConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BooleanManagedProperty_True_CannotChange_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_boolmanaged\":\"True|False\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<BooleanManagedProperty>(json, new BooleanManagedPropertyConverter());

            // Assert
            Assert.True(actual.Value);
            Assert.False(actual.CanBeChanged);
        }

        [Fact]
        public void BooleanManagedProperty_False_CanChange_Can_Serialize()
        {
            // Arrange
            var value = new BooleanManagedProperty(false) { CanBeChanged = true };
            var expected = "{\"_boolmanaged\":\"False|True\"}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new BooleanManagedPropertyConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BooleanManagedProperty_False_CanChange_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_boolmanaged\":\"False|True\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<BooleanManagedProperty>(json, new BooleanManagedPropertyConverter());

            // Assert
            Assert.False(actual.Value);
            Assert.True(actual.CanBeChanged);
        }

        [Fact]
        public void BooleanManagedProperty_False_CannotChange_Can_Serialize()
        {
            // Arrange
            var value = new BooleanManagedProperty(false) { CanBeChanged = false };
            var expected = "{\"_boolmanaged\":\"False|False\"}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new BooleanManagedPropertyConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BooleanManagedProperty_False_CannotChange_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_boolmanaged\":\"False|False\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<BooleanManagedProperty>(json, new BooleanManagedPropertyConverter());

            // Assert
            Assert.False(actual.Value);
            Assert.False(actual.CanBeChanged);
        }

        [Fact]
        public void BooleanManagedProperty_DefaultCanBeChanged_Can_Deserialize()
        {
            // Arrange - only value, no CanBeChanged
            var json = "{\"_boolmanaged\":\"True\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<BooleanManagedProperty>(json, new BooleanManagedPropertyConverter());

            // Assert
            Assert.True(actual.Value);
            Assert.True(actual.CanBeChanged); // Default is true
        }

        #endregion Public Methods
    }
}
