namespace Xrm.Json.Serialization.Tests
{
    using Xrm.Json.Serialization;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using Xunit;

    public class OptionSetValueConverterTests
    {
        #region Public Methods

        [Fact]
        public void OptionSetValue_Can_Deserialize()
        {
            // Arrange
            var value = "{\"_option\":100}";
            var expected = new OptionSetValue(100);

            // Act
            var actual = JsonConvert.DeserializeObject<OptionSetValue>(value, new OptionSetConverter());

            // Assert
            Assert.Equal(expected.Value, actual.Value);
        }

        [Fact]
        public void OptionSetValue_Can_Serialize()
        {
            // Arrange
            var value = new OptionSetValue(100);
            var expected = "{\"_option\":100}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new OptionSetConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion Public Methods
    }
}