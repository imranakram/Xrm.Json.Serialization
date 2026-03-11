namespace Xrm.Json.Serialization
{
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;

    public static class EntitySerializer
    {
        #region Private Fields

        private static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
        {
            ContractResolver = new XrmContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        private static readonly JsonSerializerSettings IndentedSettings = new JsonSerializerSettings
        {
            ContractResolver = new XrmContractResolver(),
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        #endregion Private Fields

        #region Public Methods

        public static string Serialize(Entity entity, bool indented = false)
        {
            return JsonConvert.SerializeObject(entity, indented ? IndentedSettings : DefaultSettings);
        }

        public static string Serialize(EntityCollection collection, bool indented = false)
        {
            return JsonConvert.SerializeObject(collection, indented ? IndentedSettings : DefaultSettings);
        }

        public static Entity DeserializeEntity(string json)
        {
            return JsonConvert.DeserializeObject<Entity>(json, DefaultSettings);
        }

        public static EntityCollection DeserializeCollection(string json)
        {
            return JsonConvert.DeserializeObject<EntityCollection>(json, DefaultSettings);
        }

        #endregion Public Methods
    }
}
