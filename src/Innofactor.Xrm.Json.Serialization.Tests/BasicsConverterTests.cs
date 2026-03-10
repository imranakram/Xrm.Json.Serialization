namespace Xrm.Json.Serialization.Tests
{
    using System.Globalization;
    using Xrm.Json.Serialization;
    using Newtonsoft.Json;
    using Xunit;

    public class BasicsConverterTests
    {
        #region Public Methods

        [Fact]
        public void Decimal_Can_Deserialize()
        {
            // Arrange
            var expected = 0.1234567890m;
            var value = $"{{\"testProp\":\"0.1234567890\"}}";

            // Act
            var actual = JsonConvert.DeserializeObject<decimal>(value, new BasicsConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decimal_Can_Serialize()
        {
            // Arrange
            var value = 0.1234567890m;
            var expected = "0.1234567890";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new BasicsConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Double_Can_Deserialize()
        {
            // Arrange
            var expected = 13.37d;
            var value = $"{{\"testProp\":{expected.ToString(CultureInfo.InvariantCulture)}}}";

            // Act
            var actual = JsonConvert.DeserializeObject<double>(value, new BasicsConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Double_Can_Serialize()
        {
            // Arrange
            var value = 13.37f;
            var expected = value.ToString(CultureInfo.InvariantCulture);

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new BasicsConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Int_Can_Deserialize()
        {
            // Arrange
            var expected = 42;
            var value = $"{{\"testProp\":{expected}}}";

            // Act
            var actual = JsonConvert.DeserializeObject<int>(value, new BasicsConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Int_Can_Serialize()
        {
            // Arrange
            var value = 42;
            var expected = $"{value}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new BasicsConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Object_Can_Deserialize()
        {
            // Arrange
            var expected = (object)"{}";
            var value = "{\"testProp\":\"{}\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<object>(value, new BasicsConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Object_Can_Serialize()
        {
            // Arrange
            var value = new object();
            var expected = "\"{}\"";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new BasicsConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void String_Can_Deserialize()
        {
            // Arrange
            var expected = "testString";
            var value = $"{{\"testProp\":\"{expected}\"}}";

            // Act
            var actual = JsonConvert.DeserializeObject<string>(value, new BasicsConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void String_Can_Serialize()
        {
            // Arrange
            var value = "test\"String";
            var expected = $"\"test\"String\"";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new BasicsConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion Public Methods
    }
}