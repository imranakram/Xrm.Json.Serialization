namespace Xrm.Json.Serialization.Tests
{
    using System;
    using Newtonsoft.Json;
    using Xunit;

    public class DateTimeConverterTests
    {
        #region Public Methods

        [Fact]
        public void DateTime_Can_Deserialize()
        {
            // Arrange
            var expected = DateTime.Now;
            var value = $"{{\"_moment\":\"{DateTimeConverter.Format(expected)}\"}}";

            // Act
            var actual = JsonConvert.DeserializeObject<DateTime>(value, new DateTimeConverter());

            // Assert
            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public void DateTime_Can_Serialize()
        {
            // Arrange
            var value = DateTime.Now;
            var expected = $"{{\"_moment\":\"{DateTimeConverter.Format(value)}\"}}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new DateTimeConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion Public Methods
    }
}