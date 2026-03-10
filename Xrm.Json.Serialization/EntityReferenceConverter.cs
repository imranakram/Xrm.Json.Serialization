namespace Xrm.Json.Serialization
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;

    public class EntityReferenceConverter : JsonConverter
    {
        #region Public Methods

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(EntityReference);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            var value = GetReference(reader);
            reader.Read();

            return value;
        }
            

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var reference = value as EntityReference;

            writer.WriteStartObject();
            writer.WritePropertyName("_reference");
            writer.WriteValue($"{reference?.LogicalName}:{reference?.Id.ToString()}");
            writer.WriteEndObject();
        }

        #endregion Public Methods

        #region Internal Methods

        internal static EntityReference GetReference(JsonReader reader)
        {
            var chunks = reader.ReadAsString().Split(':');
            var name = string.Empty;
            var id = Guid.Empty;

            if (chunks.Length > 1)
            {
                name = chunks[0];
                Guid.TryParse(chunks[1], out id);
            }

            return new EntityReference(name, id);
        }

        #endregion Internal Methods
    }
}