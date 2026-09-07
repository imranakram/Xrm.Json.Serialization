namespace Xrm.Json.Serialization
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xrm.Sdk;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    internal class XrmContractResolver : DefaultContractResolver
    {
        #region Internal Fields

        /// <summary>
        /// The single resolver instance every converter in this assembly routes through.
        /// <para>
        /// <see cref="DefaultContractResolver"/> caches the <see cref="Newtonsoft.Json.Serialization.JsonContract"/>
        /// it builds for each type, and that cache lives on the instance. Handing out a fresh
        /// instance per serialize call therefore threw the cache away every time and forced
        /// every type on the graph to be re-resolved by reflection. Sharing one instance is
        /// safe: <see cref="converters"/> is never mutated after construction, and the base
        /// class's contract cache is thread-safe.
        /// </para>
        /// </summary>
        internal static readonly XrmContractResolver Shared = new XrmContractResolver();

        #endregion Internal Fields

        #region Private Fields

        /// <summary>
        /// Fallback converter for every type without a dedicated one. Held as a single instance
        /// because it carries no state, and <see cref="ResolveContractConverter"/> is on the
        /// path that <see cref="Shared"/> exists to keep cheap.
        /// </summary>
        private static readonly BasicsConverter Basics = new BasicsConverter();

        private readonly Dictionary<Type, JsonConverter> converters;

        #endregion Private Fields

        #region Public Constructors

        public XrmContractResolver()
        {
            converters = new Dictionary<Type, JsonConverter>()
            {
                { typeof(AliasedValue), new AliasedValueConverter() },
                { typeof(BooleanManagedProperty), new BooleanManagedPropertyConverter() },
                { typeof(DateTime), new DateTimeConverter()},
                { typeof(Entity), new EntityConverter() },
                { typeof(EntityCollection), new EntityCollectionConverter() },
                { typeof(EntityReference), new EntityReferenceConverter() },
                { typeof(Guid), new GuidConverter() },
                { typeof(Money), new MoneyConverter() },
                { typeof(OptionSetValue), new OptionSetConverter()},
                { typeof(OptionSetValueCollection), new OptionSetValueCollectionConverter() }
            };
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (!converters.TryGetValue(objectType, out var matchingConverter))
            {
                return Basics;
            }

            return matchingConverter;
        }

        #endregion Protected Methods

    }
}