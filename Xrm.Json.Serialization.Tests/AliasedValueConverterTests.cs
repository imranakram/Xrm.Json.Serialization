namespace Xrm.Json.Serialization.Tests
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using Xunit;

    public class AliasedValueConverterTests
    {
        #region Public Methods

        [Fact]
        public void AliasedValue_String_Can_Serialize()
        {
            // Arrange
            var value = new AliasedValue("account", "name", "Contoso Ltd");
            var expected = "{\"_aliased\":\"account|name|Contoso Ltd\"}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new AliasedValueConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AliasedValue_String_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_aliased\":\"account|name|Contoso Ltd\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<AliasedValue>(json, new AliasedValueConverter());

            // Assert
            Assert.Equal("account", actual.EntityLogicalName);
            Assert.Equal("name", actual.AttributeLogicalName);
            Assert.Equal("Contoso Ltd", actual.Value);
        }

        [Fact]
        public void AliasedValue_EntityReference_Can_Serialize()
        {
            // Arrange
            var entityRef = new EntityReference("contact", Guid.Parse("12345678-1234-1234-1234-123456789012"));
            var value = new AliasedValue("account", "primarycontactid", entityRef);
            var expected = "{\"_aliased\":\"account|primarycontactid|ref:contact:12345678-1234-1234-1234-123456789012\"}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new AliasedValueConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AliasedValue_EntityReference_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_aliased\":\"account|primarycontactid|ref:contact:12345678-1234-1234-1234-123456789012\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<AliasedValue>(json, new AliasedValueConverter());

            // Assert
            Assert.Equal("account", actual.EntityLogicalName);
            Assert.Equal("primarycontactid", actual.AttributeLogicalName);
            Assert.IsType<EntityReference>(actual.Value);
            var entityRef = (EntityReference)actual.Value;
            Assert.Equal("contact", entityRef.LogicalName);
            Assert.Equal(Guid.Parse("12345678-1234-1234-1234-123456789012"), entityRef.Id);
        }

        [Fact]
        public void AliasedValue_OptionSet_Can_Serialize()
        {
            // Arrange
            var optionSet = new OptionSetValue(1);
            var value = new AliasedValue("account", "industrycode", optionSet);
            var expected = "{\"_aliased\":\"account|industrycode|opt:1\"}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new AliasedValueConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AliasedValue_OptionSet_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_aliased\":\"account|industrycode|opt:1\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<AliasedValue>(json, new AliasedValueConverter());

            // Assert
            Assert.Equal("account", actual.EntityLogicalName);
            Assert.Equal("industrycode", actual.AttributeLogicalName);
            Assert.IsType<OptionSetValue>(actual.Value);
            Assert.Equal(1, ((OptionSetValue)actual.Value).Value);
        }

        [Fact]
        public void AliasedValue_Money_Can_Serialize()
        {
            // Arrange
            var money = new Money(1000000m);
            var value = new AliasedValue("account", "revenue", money);
            var expected = "{\"_aliased\":\"account|revenue|mon:1000000\"}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new AliasedValueConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AliasedValue_Money_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_aliased\":\"account|revenue|mon:1000000\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<AliasedValue>(json, new AliasedValueConverter());

            // Assert
            Assert.Equal("account", actual.EntityLogicalName);
            Assert.Equal("revenue", actual.AttributeLogicalName);
            Assert.IsType<Money>(actual.Value);
            Assert.Equal(1000000m, ((Money)actual.Value).Value);
        }

        [Fact]
        public void AliasedValue_Guid_Can_Serialize()
        {
            // Arrange
            var guid = Guid.Parse("12345678-1234-1234-1234-123456789012");
            var value = new AliasedValue("account", "accountid", guid);
            var expected = "{\"_aliased\":\"account|accountid|guid:12345678-1234-1234-1234-123456789012\"}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new AliasedValueConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AliasedValue_Guid_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_aliased\":\"account|accountid|guid:12345678-1234-1234-1234-123456789012\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<AliasedValue>(json, new AliasedValueConverter());

            // Assert
            Assert.Equal("account", actual.EntityLogicalName);
            Assert.Equal("accountid", actual.AttributeLogicalName);
            Assert.IsType<Guid>(actual.Value);
            Assert.Equal(Guid.Parse("12345678-1234-1234-1234-123456789012"), actual.Value);
        }

        [Fact]
        public void AliasedValue_Boolean_Can_Serialize()
        {
            // Arrange
            var value = new AliasedValue("account", "donotphone", true);
            var expected = "{\"_aliased\":\"account|donotphone|bool:True\"}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new AliasedValueConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AliasedValue_Boolean_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_aliased\":\"account|donotphone|bool:True\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<AliasedValue>(json, new AliasedValueConverter());

            // Assert
            Assert.Equal("account", actual.EntityLogicalName);
            Assert.Equal("donotphone", actual.AttributeLogicalName);
            Assert.IsType<bool>(actual.Value);
            Assert.True((bool)actual.Value);
        }

        [Fact]
        public void AliasedValue_Null_Can_Serialize()
        {
            // Arrange
            var value = new AliasedValue("account", "parentaccountid", null);
            var expected = "{\"_aliased\":\"account|parentaccountid|\"}";

            // Act
            var actual = JsonConvert.SerializeObject(value, Formatting.None, new AliasedValueConverter());

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AliasedValue_Null_Can_Deserialize()
        {
            // Arrange
            var json = "{\"_aliased\":\"account|parentaccountid|\"}";

            // Act
            var actual = JsonConvert.DeserializeObject<AliasedValue>(json, new AliasedValueConverter());

            // Assert
            Assert.Equal("account", actual.EntityLogicalName);
            Assert.Equal("parentaccountid", actual.AttributeLogicalName);
            Assert.Null(actual.Value);
        }

        #endregion Public Methods
    }
}
