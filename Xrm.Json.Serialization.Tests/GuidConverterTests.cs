namespace Xrm.Json.Serialization.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Xunit;

    public class GuidConverterTests
    {
        [Fact]
        public void Guid_Can_Deserialize()
        {
            // Arrange
            var expected = Guid.NewGuid();
            var value = $"{{\"_id\":\"{expected.ToString()}\"}}";

            // Act
            var actual = JsonConvert.DeserializeObject<Guid>(value, new GuidConverter());

            // Assert
            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public void Guid_Can_Serialize()
        {
            // Arrange
            var value = Guid.NewGuid();
            var expected = $"{{\"_id\":\"{value.ToString()}\"}}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new GuidConverter());

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
