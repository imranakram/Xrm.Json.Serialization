namespace Xrm.Json.Serialization
{
    using System;
    using Newtonsoft.Json;

    public class DateTimeConverter : JsonConverter
    {
        #region Public Methods

        public static string Format(DateTime moment)
        {
            string offset(DateTime value)
            {
                var result = TimeZone.CurrentTimeZone.GetUtcOffset(value);
                return ((result < TimeSpan.Zero) ? "-" : "+") + result.ToString("hhmm");
            }

            return moment.ToString("yyyy-MM-dd HH:mm:ss") + offset(moment);
        }

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(DateTime);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Read();
            var value = DateTime.Parse(reader.ReadAsString());
            reader.Read();

            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("_moment");
            writer.WriteValue(Format((DateTime)value));
            writer.WriteEndObject();
        }

        #endregion Public Methods
    }
}