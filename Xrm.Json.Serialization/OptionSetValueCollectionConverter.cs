namespace Xrm.Json.Serialization
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;

    public class OptionSetValueCollectionConverter : JsonConverter
    {
        #region Public Methods

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(OptionSetValueCollection);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Read();

            if (reader.TokenType != JsonToken.PropertyName || reader.Value?.ToString() != "_options")
            {
                throw new JsonException("Expected _options property");
            }

            reader.Read();

            if (reader.TokenType != JsonToken.StartArray)
            {
                throw new JsonException("Expected array for option set values");
            }

            var collection = new OptionSetValueCollection();

            reader.Read();

            while (reader.TokenType != JsonToken.EndArray)
            {
                if (reader.TokenType == JsonToken.Integer)
                {
                    collection.Add(new OptionSetValue((int)(long)reader.Value));
                }

                reader.Read();
            }

            reader.Read();

            return collection;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var collection = value as OptionSetValueCollection;

            writer.WriteStartObject();
            writer.WritePropertyName("_options");
            writer.WriteStartArray();

            if (collection != null)
            {
                foreach (var optionValue in collection)
                {
                    writer.WriteValue(optionValue.Value);
                }
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        #endregion Public Methods
    }
}
