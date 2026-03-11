namespace Xrm.Json.Serialization
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;

    public class AliasedValueConverter : JsonConverter
    {
        #region Public Methods

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(AliasedValue);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Read();

            if (reader.TokenType != JsonToken.PropertyName || reader.Value?.ToString() != "_aliased")
            {
                throw new JsonException("Expected _aliased property");
            }

            reader.Read();

            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonException("Expected string value for _aliased");
            }

            var serializedValue = reader.Value?.ToString() ?? string.Empty;
            var parts = serializedValue.Split('|');
            if (parts.Length < 3)
            {
                throw new JsonException("Invalid aliased value format. Expected: entityLogicalName|attributeLogicalName|value");
            }

            var entityLogicalName = parts[0];
            var attributeLogicalName = parts[1];
            var value = DeserializeValue(parts[2], reader, serializer);

            reader.Read();

            return new AliasedValue(entityLogicalName, attributeLogicalName, value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var aliasedValue = value as AliasedValue;

            writer.WriteStartObject();
            writer.WritePropertyName("_aliased");

            var serializedValue = SerializeValue(aliasedValue.Value, serializer);
            writer.WriteValue($"{aliasedValue.EntityLogicalName}|{aliasedValue.AttributeLogicalName}|{serializedValue}");

            writer.WriteEndObject();
        }

        #endregion Public Methods

        #region Private Methods

        private string SerializeValue(object value, JsonSerializer serializer)
        {
            if (value == null)
                return string.Empty;

            if (value is EntityReference entityRef)
                return $"ref:{entityRef.LogicalName}:{entityRef.Id}";
            if (value is OptionSetValue optionSet)
                return $"opt:{optionSet.Value}";
            if (value is Money money)
                return $"mon:{money.Value}";
            if (value is DateTime dateTime)
                return $"dt:{dateTime:O}";
            if (value is Guid guid)
                return $"guid:{guid}";
            if (value is bool boolean)
                return $"bool:{boolean}";

            return value.ToString();
        }

        private object DeserializeValue(string serialized, JsonReader reader, JsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(serialized))
                return null;

            if (serialized.StartsWith("ref:"))
            {
                var parts = serialized.Substring(4).Split(':');
                if (parts.Length == 2)
                    return new EntityReference(parts[0], Guid.Parse(parts[1]));
            }
            else if (serialized.StartsWith("opt:"))
            {
                return new OptionSetValue(int.Parse(serialized.Substring(4)));
            }
            else if (serialized.StartsWith("mon:"))
            {
                return new Money(decimal.Parse(serialized.Substring(4)));
            }
            else if (serialized.StartsWith("dt:"))
            {
                return DateTime.Parse(serialized.Substring(3));
            }
            else if (serialized.StartsWith("guid:"))
            {
                return Guid.Parse(serialized.Substring(5));
            }
            else if (serialized.StartsWith("bool:"))
            {
                return bool.Parse(serialized.Substring(5));
            }

            return serialized;
        }

        #endregion Private Methods
    }
}
