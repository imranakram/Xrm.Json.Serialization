namespace Xrm.Json.Serialization
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;

    public class MoneyConverter : JsonConverter
    {
        #region Public Methods

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(Money);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            var value = new Money((decimal)reader.ReadAsDecimal());
            reader.Read();

            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("_money");
            writer.WriteValue((value as Money).Value);
            writer.WriteEndObject();
        }

        #endregion Public Methods
    }
}