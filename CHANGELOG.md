# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project uses **Calendar Versioning (CalVer)**: `1.YYYY.MM.patch`

## [1.2026.9.0] - 2026-09-07

### Fixed
- **Bulk serialization performance** - `EntityConverter` and `EntityCollectionConverter` assigned
  `serializer.ContractResolver = new XrmContractResolver()` on every call. Newtonsoft's
  `DefaultContractResolver` caches the `JsonContract` it builds for each type on the instance, so a
  fresh instance per call discarded that cache and forced every type on the graph to be re-resolved
  by reflection. The converters now route through a single shared resolver.
  Measured at 100 000 entities of five attributes each (.NET Framework 4.8, x64): **82.31 s -> 0.34 s**,
  byte-identical output. For reference, plain reflective Newtonsoft on the same objects takes 1.03 s.
- `XrmContractResolver.ResolveContractConverter` no longer allocates a `BasicsConverter` per
  resolution for types without a dedicated converter.

### Added
- 9 tests in `ContractResolverCachingTests` covering resolver sharing across repeated and nested
  calls, replacement of a foreign resolver, a round trip over all 13 supported attribute types, and
  a bulk-throughput guard that fails if the per-call allocation returns.
- `Performance` section in the README, including guidance on reusing a `JsonSerializerSettings`
  instance and the `JsonConvert.DefaultSettings` factory pitfall.

### Changed
- `InternalsVisibleTo("Xrm.Json.Serialization.Tests")` so the tests can assert on the shared
  resolver instance without making `XrmContractResolver` part of the public API.

### Compatibility
No change to the JSON format, the public API or the target framework. This is a drop-in upgrade.

## [1.2026.3.0] - 2026-03-11

### Added
- **AliasedValue converter** - Critical for FetchXML queries with linked entities
- **OptionSetValueCollection converter** - Support for multi-select picklists
- **BooleanManagedProperty converter** - Support for managed property fields
- **EntitySerializer helper class** - Simplified serialization/deserialization API
- 27 new comprehensive unit tests for new converters
- Plugin usage examples in README (logging, Service Bus, webhooks, FetchXML, caching)
- External integration examples (console apps, Azure Functions, Web API)
- Deployment guidance for plugins (ILMerge vs NuGet package approaches)

### Fixed
- **[#20]** String escaping issue with double quotes and special characters in `BasicsConverter`
- Removed unsafe `CodeDomProvider` usage that caused JSON escaping problems
- Proper handling of backslashes, newlines, and tabs in strings

### Changed
- **Reverted target framework** from .NET Framework 4.8 to **4.6.2** for plugin compatibility
- Updated `XrmContractResolver` to include new converters
- Enhanced `EntityConverter` to support deserializing new data types
- Updated xunit.runner.visualstudio from 3.1.5 to 2.5.3 (net462 compatibility)
- Improved README with comprehensive plugin and external integration examples
- Updated NuGet package metadata with better description and tags

## [2.0.0] - 2025-03-10

### Changed
- **BREAKING:** Changed root namespace from `Innofactor.Xrm.Json.Serialization` to `Xrm.Json.Serialization`
- **BREAKING:** Assembly name changed from `Innofactor.Xrm.Json.Serialization.dll` to `Xrm.Json.Serialization.dll`
- Upgraded Newtonsoft.Json from 13.0.1 to 13.0.4
- Upgraded System.Text.Json from 6.0.6 to 6.0.10
- Upgraded xUnit from 2.4.2 to 2.9.3
- Updated assembly metadata (copyright 2025)
- Improved NuGet package metadata with better description and tags
- Project structure: moved projects from `src\` folder to root level

### Added
- Comprehensive README.md with usage examples and documentation
- CHANGELOG.md for version history tracking

## [1.0.0] - 2021

### Added
- Initial release
- Support for Dynamics 365/CRM entity serialization
- Converters for Entity, EntityCollection, EntityReference
- Converters for OptionSetValue, Money, DateTime, Guid
- BasicsConverter for primitive types (string, int, double, decimal)
- XrmContractResolver for automatic converter selection
- Comprehensive unit test coverage

### Supported Types
- `Entity` - Full entity serialization with attributes
- `EntityCollection` - Collections of entities
- `EntityReference` - Lookup/reference fields
- `OptionSetValue` - Picklist values
- `Money` - Currency values
- `DateTime` - Date/time with timezone support
- `Guid` - Unique identifiers
- Basic CLR types (string, int, long, float, double, decimal, object)

---

---

## Versioning

This project uses **Calendar Versioning (CalVer)**: `1.YYYY.MM.patch`

Format: `Major.Year.Month.Patch`

Examples:
- `1.2026.3.0` - March 2026, first release
- `1.2026.3.1` - March 2026, patch release
- `1.2022.10.1` - October 2022

---

## Version Comparison

| Version | Date | .NET Framework | Key Changes |
|---------|------|----------------|-------------|
| **1.2026.3.0** | 2026-03-11 | 4.6.2 | AliasedValue, multi-select, plugin examples, bug #20 fix |
| 1.2022.10.1 | 2022-10 | 4.6.2 | Namespace change to Xrm.Json.Serialization |
| 1.0.0 | 2021 | 4.6.2 | Initial release |

---

## Links

- [NuGet Package](https://www.nuget.org/packages/Xrm.Json.Serialization/)
- [GitHub Repository](https://github.com/imranakram/Xrm.Json.Serialization)
- [Issue Tracker](https://github.com/imranakram/Xrm.Json.Serialization/issues)
- [Documentation](https://github.com/imranakram/Xrm.Json.Serialization/blob/main/README.md)
