namespace Xrm.Json.Serialization.Tests
{
    using Xrm.Json.Serialization;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using Xunit;

    public class MoneyConverterTests
    {
        #region Public Methods

        [Fact]
        public void Money_Can_Deserialize()
        {
            // Arrange
            var expected = new Money(9.95m);
            var value = "{\"_money\":9.95}";

            // Act
            var actual = JsonConvert.DeserializeObject<Money>(value, new MoneyConverter());

            // Assert
            Assert.Equal(expected.Value, actual.Value);
        }

        [Fact]
        public void Money_Can_Serialize()
        {
            // Arrange
            var value = new Money(9.95m);
            var expected = "{\"_money\":9.95}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new MoneyConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion Public Methods
    }
}