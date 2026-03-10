namespace Xrm.Json.Serialization
{
    using System;
    using Newtonsoft.Json;

    public class GuidConverter : JsonConverter
    {
        #region Public Methods

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(Guid);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            var value = Guid.Parse(reader.ReadAsString());
            reader.Read();

            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("_id");
            writer.WriteValue(((Guid)value).ToString());
            writer.WriteEndObject();
        }

        #endregion Public Methods
    }
}