# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2025-03-10

### 🎉 Major Release - Breaking Changes

This release represents a major refactoring of the library with breaking namespace changes.

### Changed
- **BREAKING:** Changed root namespace from `Innofactor.Xrm.Json.Serialization` to `Xrm.Json.Serialization`
- **BREAKING:** Upgraded target framework from .NET Framework 4.6.2 to .NET Framework 4.8
- **BREAKING:** Assembly name changed from `Innofactor.Xrm.Json.Serialization.dll` to `Xrm.Json.Serialization.dll`
- Upgraded Newtonsoft.Json from 13.0.1 to 13.0.3
- Upgraded System.Text.Json from 6.0.6 to 6.0.10
- Upgraded xUnit from 2.4.2 to 2.9.3
- Updated assembly metadata (Biznamics branding, copyright 2025)
- Improved NuGet package metadata with better description and tags
- Project structure: moved projects from `src\` folder to root level

### Added
- Comprehensive README.md with usage examples and documentation
- UPGRADE-GUIDE.md with detailed migration instructions
- CHANGELOG.md for version history tracking
- fix-structure.ps1 PowerShell script for project restructuring

### Migration Guide
To upgrade from 1.x to 2.0:

1. **Update NuGet package:**
   ```powershell
   Update-Package Xrm.Json.Serialization
   ```

2. **Update using statements:**
   ```csharp
   // Old
   using Innofactor.Xrm.Json.Serialization;
   
   // New
   using Xrm.Json.Serialization;
   ```

3. **Update project target framework** (if needed):
   - Ensure your project targets .NET Framework 4.8 or higher

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

## Version History

| Version | Date | .NET Framework | Newtonsoft.Json | CRM SDK |
|---------|------|----------------|-----------------|---------|
| 2.0.0 | 2025-03-10 | 4.8 | 13.0.3 | 9.0.2.46 |
| 1.0.0 | 2021 | 4.6.2 | 13.0.1 | 9.0.2.46 |

---

## Links

- [NuGet Package](https://www.nuget.org/packages/Xrm.Json.Serialization/)
- [GitHub Repository](https://github.com/Biznamics/Xrm.Json.Serialization)
- [Issue Tracker](https://github.com/Biznamics/Xrm.Json.Serialization/issues)
- [Documentation](https://github.com/Biznamics/Xrm.Json.Serialization/blob/main/README.md)
