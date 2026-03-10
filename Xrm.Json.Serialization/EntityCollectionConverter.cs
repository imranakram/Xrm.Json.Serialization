namespace Xrm.Json.Serialization
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;

    public class EntityCollectionConverter : JsonConverter
    {
        #region Public Methods

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(EntityCollection);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Read();

            serializer.ContractResolver = new XrmContractResolver();

            var result = new EntityCollection();

            while (reader.TokenType != JsonToken.EndArray)
            {
                result.Entities.Add(serializer.Deserialize<Entity>(reader));

                // Skipping closing object definition
                reader.Read();
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.ContractResolver = new XrmContractResolver();

            var collection = value as EntityCollection;

            writer.WriteStartArray();

            foreach (var entity in collection?.Entities)
            {
                serializer.Serialize(writer, entity);
            }

            writer.WriteEndArray();
        }

        #endregion Public Methods
    }
}