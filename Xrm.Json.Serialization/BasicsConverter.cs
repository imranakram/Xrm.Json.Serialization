namespace Xrm.Json.Serialization
{
    using System;
    using Newtonsoft.Json;

    public class BasicsConverter : JsonConverter
    {
        #region Public Methods

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string)
                || objectType == typeof(int)
                || objectType == typeof(long)
                || objectType == typeof(float)
                || objectType == typeof(double)
                || objectType == typeof(decimal)
                || objectType == typeof(object);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Read();

            if (objectType == typeof(string))
            {
                return Finish(reader, reader.ReadAsString());
            }

            if (objectType == typeof(int) || objectType == typeof(long))
            {
                // MS Dynamics CRM has no `long` type, only `int`
                return Finish(reader, (int)reader.ReadAsInt32());
            }

            if (objectType == typeof(double) || objectType == typeof(float))
            {
                // MS Dynamics CRM has no `float` type, only `double`
                return Finish(reader, (double)reader.ReadAsDouble());
            }

            if (objectType == typeof(decimal))
            {
                return Finish(reader, (decimal)reader.ReadAsDecimal());
            }

            // Default to object rep
            reader.Read();
            return Finish(reader, reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null && value.GetType() == typeof(object))
            {
                // For generic object type, serialize using JsonConvert
                writer.WriteValue(JsonConvert.SerializeObject(value));
            }
            else
            {
                // For all other types (string, int, double, decimal, etc.)
                // JsonWriter.WriteValue handles proper escaping automatically
                writer.WriteValue(value);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private T Finish<T>(JsonReader reader, T value)
        {
            reader.Read();
            return value;
        }

        #endregion Private Methods
    }
}