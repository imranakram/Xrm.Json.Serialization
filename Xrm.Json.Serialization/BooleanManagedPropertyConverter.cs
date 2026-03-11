namespace Xrm.Json.Serialization
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;

    public class BooleanManagedPropertyConverter : JsonConverter
    {
        #region Public Methods

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(BooleanManagedProperty);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Read();

            if (reader.TokenType != JsonToken.PropertyName || reader.Value?.ToString() != "_boolmanaged")
            {
                throw new JsonException("Expected _boolmanaged property");
            }

            reader.Read();

            var serializedValue = reader.Value?.ToString() ?? string.Empty;
            var parts = serializedValue.Split('|');
            var value = parts.Length > 0 && bool.Parse(parts[0]);
            var canBeChanged = parts.Length > 1 ? bool.Parse(parts[1]) : true;

            reader.Read();

            return new BooleanManagedProperty(value) { CanBeChanged = canBeChanged };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var managedProperty = value as BooleanManagedProperty;

            writer.WriteStartObject();
            writer.WritePropertyName("_boolmanaged");
            writer.WriteValue($"{managedProperty.Value}|{managedProperty.CanBeChanged}");
            writer.WriteEndObject();
        }

        #endregion Public Methods
    }
}
