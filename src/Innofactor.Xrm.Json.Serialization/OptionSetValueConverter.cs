namespace Xrm.Json.Serialization
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;

    public class OptionSetConverter : JsonConverter
    {
        #region Public Methods

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(OptionSetValue);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            var value = new OptionSetValue((int)reader.ReadAsInt32());
            reader.Read();

            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("_option");
            writer.WriteValue((value as OptionSetValue).Value);
            writer.WriteEndObject();
        }

        #endregion Public Methods
    }
}