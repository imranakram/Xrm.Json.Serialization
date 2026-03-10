namespace Xrm.Json.Serialization
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;

    public class EntityConverter : JsonConverter
    {
        #region Public Methods

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(Entity);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.Read();

            var reference = EntityReferenceConverter.GetReference(reader);
            var entity = new Entity(reference.LogicalName, reference.Id);

            reader.Read();

            var key = default(string);

            try
            {
                while (reader.TokenType != JsonToken.EndObject)
                {
                    // Reading attribute name
                    key = reader.Value.ToString();
                    var value = default(object);

                    // Noving to next token
                    reader.Read();
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        // Skipping to first property of the object
                        reader.Read();

                        switch (reader.Value)
                        {
                            case "_option":
                                // Skipping to property value of the object
                                value = new OptionSetValue((int)reader.ReadAsInt32());
                                reader.Read();
                                break;

                            case "_reference":
                                // Skipping to property value of the object
                                value = EntityReferenceConverter.GetReference(reader);
                                reader.Read();
                                break;

                            case "_money":
                                // Skipping to property value of the object
                                value = new Money((decimal)reader.ReadAsDecimal());
                                reader.Read();
                                break;

                            case "_moment":
                                // Skipping to property value of the object
                                value = DateTime.Parse(reader.ReadAsString());
                                reader.Read();
                                break;

                            case "_id":
                                // Skipping to property value of the object
                                value = Guid.Parse(reader.ReadAsString());
                                reader.Read();
                                break;
                        }
                    }
                    else
                    {
                        if (reader.Value.GetType() == typeof(long))
                        {
                            // Trying to downscale `long` to `int` if possible
                            var temp = (long)reader.Value;

                            if (temp > int.MaxValue || temp < int.MinValue)
                            {
                                value = (long)reader.Value;
                            }
                            else
                            {
                                value = unchecked((int)temp);
                            }
                        }
                        else if (reader.Value.GetType() == typeof(float))
                        {
                            // Trying to upscale `float` to `double`
                            value = (double)reader.Value;
                        }
                        else
                        {
                            value = serializer.Deserialize(reader);
                        }
                    }

                    entity.Attributes.Add(key, value);

                    // Skipping closing object definition
                    reader.Read();
                }
            }
            catch (Exception ex)
            {
                throw new JsonException($"Error deserializing property `{key}`", ex);
            }

            return entity;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.ContractResolver = new XrmContractResolver();

            var entity = value as Entity;

            writer.WriteStartObject();
            writer.WritePropertyName("_reference");
            writer.WriteValue($"{entity?.LogicalName}:{entity?.Id.ToString()}");

            foreach (var attribute in entity?.Attributes)
            {
                if (attribute.Value != null)
                {
                    // If attribute is set to `null` that is equivalent to removing attribute from collection
                    writer.WritePropertyName(attribute.Key);
                    serializer.Serialize(writer, attribute.Value);
                }
            }

            writer.WriteEndObject();
        }

        #endregion Public Methods
    }
}