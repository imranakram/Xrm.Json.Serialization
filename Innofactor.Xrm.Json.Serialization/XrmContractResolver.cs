namespace Xrm.Json.Serialization
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    internal class XrmContractResolver : DefaultContractResolver
    {
        #region Private Fields

        private readonly Dictionary<Type, JsonConverter> converters;

        #endregion Private Fields

        #region Public Constructors

        public XrmContractResolver()
        {
            converters = new Dictionary<Type, JsonConverter>()
            {
                { typeof(DateTime), new DateTimeConverter()},
                { typeof(Entity), new EntityConverter() },
                { typeof(EntityCollection), new EntityCollectionConverter() },
                { typeof(EntityReference), new EntityReferenceConverter() },
                { typeof(Guid), new GuidConverter() },
                { typeof(Money), new MoneyConverter() },
                { typeof(OptionSetValue), new OptionSetConverter()}
            };
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (!converters.TryGetValue(objectType, out var matchingConverter))
            {
                return new BasicsConverter();
            }

            return matchingConverter;
        }

        #endregion Protected Methods

    }
}