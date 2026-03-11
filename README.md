# Xrm.Json.Serialization

Compact JSON serialization library for Microsoft Dynamics 365/CRM/Dataverse entities using Newtonsoft.Json.

[![NuGet](https://img.shields.io/nuget/v/Xrm.Json.Serialization.svg)](https://www.nuget.org/packages/Xrm.Json.Serialization)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Features

- ✅ **Compact JSON format** optimized for Dynamics 365 entities
- ✅ **Complete data type support** for all major CRM SDK types
- ✅ **Custom converters** for CRM-specific types (EntityReference, OptionSetValue, Money, etc.)
- ✅ **Bidirectional** serialization and deserialization
- ✅ **Easy integration** with existing Dynamics 365 SDK projects
- ✅ Built for **.NET Framework 4.8** with Dynamics 365 SDK 9.x

## Supported Data Types

This library provides custom JSON converters for the following Dynamics 365 data types:

| Data Type | CRM SDK Type | Description | Converter |
|-----------|--------------|-------------|-----------|
| Entity | `Entity` | Complete entity records | `EntityConverter` |
| Entity Reference | `EntityReference` | References to related records | `EntityReferenceConverter` |
| Entity Collection | `EntityCollection` | Multiple entity records | `EntityCollectionConverter` |
| Option Set | `OptionSetValue` | Picklist/Choice fields | `OptionSetConverter` |
| Money | `Money` | Currency fields | `MoneyConverter` |
| Guid | `Guid` | Unique identifiers | `GuidConverter` |
| DateTime | `DateTime` | Date and time fields | `DateTimeConverter` |
| Basic Types | `string`, `int`, `double`, `decimal` | Standard primitive types | `BasicsConverter` |

## Installation

### NuGet Package Manager
```powershell
Install-Package Xrm.Json.Serialization
```

### .NET CLI
```bash
dotnet add package Xrm.Json.Serialization
```

### Package Reference
```xml
<PackageReference Include="Xrm.Json.Serialization" Version="1.0.0" />
```

## Usage

### Basic Serialization

```csharp
using Xrm.Json.Serialization;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;

// Create a Dynamics 365 entity
var account = new Entity("account", Guid.NewGuid());
account["name"] = "Contoso Ltd";
account["revenue"] = new Money(1000000m);
account["industrycode"] = new OptionSetValue(1);
account["parentaccountid"] = new EntityReference("account", Guid.NewGuid());
account["createdon"] = DateTime.UtcNow;

// Configure JSON serializer with XRM contract resolver
var settings = new JsonSerializerSettings
{
    ContractResolver = new XrmContractResolver(),
    Formatting = Formatting.Indented
};

// Serialize to JSON
string json = JsonConvert.SerializeObject(account, settings);
Console.WriteLine(json);
```

### Deserialization

```csharp
using Xrm.Json.Serialization;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;

string json = @"{
    ""_reference"": ""account:12345678-1234-1234-1234-123456789012"",
    ""name"": ""Contoso Ltd"",
    ""revenue"": {
        ""_money"": 1000000
    },
    ""industrycode"": {
        ""_option"": 1
    }
}";

var settings = new JsonSerializerSettings
{
    ContractResolver = new XrmContractResolver()
};

// Deserialize from JSON
var account = JsonConvert.DeserializeObject<Entity>(json, settings);
Console.WriteLine($"Account: {account["name"]}");
Console.WriteLine($"Revenue: {((Money)account["revenue"]).Value}");
```

### Entity Collection Serialization

```csharp
using Xrm.Json.Serialization;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;

var collection = new EntityCollection();

collection.Entities.Add(new Entity("account", Guid.NewGuid())
{
    ["name"] = "Account 1"
});

collection.Entities.Add(new Entity("account", Guid.NewGuid())
{
    ["name"] = "Account 2"
});

var settings = new JsonSerializerSettings
{
    ContractResolver = new XrmContractResolver(),
    Formatting = Formatting.Indented
};

string json = JsonConvert.SerializeObject(collection, settings);
```

### Individual Converter Usage

You can also use individual converters directly:

```csharp
// Entity Reference
var reference = new EntityReference("contact", Guid.NewGuid());
var json = JsonConvert.SerializeObject(reference, new EntityReferenceConverter());

// Money
var money = new Money(50000m);
var json = JsonConvert.SerializeObject(money, new MoneyConverter());

// OptionSet
var optionSet = new OptionSetValue(100000001);
var json = JsonConvert.SerializeObject(optionSet, new OptionSetConverter());
```

## Compact JSON Format

The library produces compact, optimized JSON representations for Dynamics 365 data types:

### Entity Reference
```json
{
  "_reference": "account:12345678-1234-1234-1234-123456789012"
}
```

### Money
```json
{
  "_money": 1000000
}
```

### OptionSetValue
```json
{
  "_option": 1
}
```

### Guid
```json
{
  "_id": "12345678-1234-1234-1234-123456789012"
}
```

### Complete Entity Example
```json
{
  "_reference": "account:12345678-1234-1234-1234-123456789012",
  "name": "Contoso Ltd",
  "revenue": {
    "_money": 1000000
  },
  "industrycode": {
    "_option": 1
  },
  "parentaccountid": {
    "_reference": "account:87654321-4321-4321-4321-210987654321"
  },
  "createdon": "2024-01-15T10:30:00Z"
}
```

## Requirements

- **.NET Framework 4.8** or higher
- **Microsoft.CrmSdk.CoreAssemblies** (>= 9.0.2.60)
- **Newtonsoft.Json** (>= 13.0.3)

## Use Cases

This library is ideal for:

- **API integrations** - Transmit Dynamics 365 data via REST APIs
- **Data export/import** - Backup and restore entity data
- **Caching** - Store entity data in Redis, file systems, or other caches
- **Logging** - Serialize entities for audit trails and debugging
- **Message queues** - Send entity data through Azure Service Bus, RabbitMQ, etc.
- **Webhooks** - Push Dynamics 365 changes to external systems

## Why Compact Format?

Traditional Dynamics 365 entity serialization can be verbose. This library provides:

- **Smaller payload sizes** - Reduced network bandwidth and storage
- **Faster serialization** - Optimized for performance
- **Easier debugging** - Human-readable JSON structure
- **Type safety** - Preserves CRM data type information

## Contributing

Contributions are welcome! Please feel free to submit issues and pull requests.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Authors

- **Alexey Shytikov** - Original author
- **Imran Akram** - Maintainer

## Links

- [GitHub Repository](https://github.com/imranakram/Xrm.Json.Serialization)
- [NuGet Package](https://www.nuget.org/packages/Xrm.Json.Serialization)
- [Issues](https://github.com/imranakram/Xrm.Json.Serialization/issues)

## Changelog

### Version 1.0.0
- Initial release
- Support for all major Dynamics 365 data types
- Entity, EntityReference, EntityCollection converters
- OptionSetValue, Money, DateTime, Guid converters
- XrmContractResolver for seamless integration
- Comprehensive test coverage
