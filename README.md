# Xrm.Json.Serialization

Compact JSON serialization library for Microsoft Dynamics 365/CRM/Dataverse entities using Newtonsoft.Json.

[![Build and Test](https://github.com/imranakram/Xrm.Json.Serialization/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/imranakram/Xrm.Json.Serialization/actions/workflows/build-and-test.yml)
[![NuGet](https://img.shields.io/nuget/v/Xrm.Json.Serialization.svg)](https://www.nuget.org/packages/Xrm.Json.Serialization)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Features

- ✅ **Compact JSON format** optimized for Dynamics 365 entities
- ✅ **Complete data type support** for all major CRM SDK types including AliasedValue
- ✅ **Custom converters** for CRM-specific types (EntityReference, OptionSetValue, Money, etc.)
- ✅ **Bidirectional** serialization and deserialization
- ✅ **Easy integration** with existing Dynamics 365 SDK projects
- ✅ Built for **.NET Framework 4.6.2** - Plugin compatible!
- ✅ **FetchXML support** - Properly handles linked entities with AliasedValue
- ✅ **Helper class** - EntitySerializer for simplified usage

## Supported Data Types

This library provides custom JSON converters for the following Dynamics 365 data types:

| Data Type | CRM SDK Type | Description | Converter |
|-----------|--------------|-------------|-----------|
| Entity | `Entity` | Complete entity records | `EntityConverter` |
| Entity Reference | `EntityReference` | References to related records | `EntityReferenceConverter` |
| Entity Collection | `EntityCollection` | Multiple entity records | `EntityCollectionConverter` |
| **Aliased Value** | `AliasedValue` | **FetchXML linked entity values** | `AliasedValueConverter` |
| Option Set | `OptionSetValue` | Picklist/Choice fields | `OptionSetConverter` |
| **Option Set Collection** | `OptionSetValueCollection` | **Multi-select picklists** | `OptionSetValueCollectionConverter` |
| **Boolean Managed Property** | `BooleanManagedProperty` | **Managed properties** | `BooleanManagedPropertyConverter` |
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
<PackageReference Include="Xrm.Json.Serialization" Version="1.2026.3.0" />
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

### Helper Class for Easy Serialization

For simplified usage, especially in plugins:

```csharp
using Xrm.Json.Serialization;
using Microsoft.Xrm.Sdk;

// Serialize
var entity = new Entity("account", Guid.NewGuid());
entity["name"] = "Contoso";

string json = EntitySerializer.Serialize(entity);
string indentedJson = EntitySerializer.Serialize(entity, indented: true);

// Deserialize
var deserializedEntity = EntitySerializer.DeserializeEntity(json);
```

## Plugin Usage

This library is **compatible with Dynamics 365 plugins** (targets .NET Framework 4.6.2), but requires special deployment considerations:

### ⚠️ Plugin Deployment Requirements

Since Dynamics 365 plugins run in an isolated sandbox, you have two options for deployment:

#### Option 1: ILMerge (Traditional)
Merge this library and Newtonsoft.Json into your plugin assembly:

```xml
<ItemGroup>
  <PackageReference Include="ILMerge" Version="3.0.41" />
  <PackageReference Include="Xrm.Json.Serialization" Version="1.1.0" />
</ItemGroup>
```

Build script:
```powershell
ILMerge.exe /out:MyPlugin.Merged.dll MyPlugin.dll Xrm.Json.Serialization.dll Newtonsoft.Json.dll /targetplatform:v4,C:\Windows\Microsoft.NET\Framework64\v4.0.30319
```

#### Option 2: NuGet Plugin Package (Modern - Recommended)
Use the Power Platform build tools to create a plugin package with dependencies:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.PowerPlatform.Dataverse.Client" Version="1.1.14" />
  <PackageReference Include="Xrm.Json.Serialization" Version="1.1.0" />
</ItemGroup>

<PropertyGroup>
  <PowerAppsTargetsPath>$(MSBuildExtensionsPath)\Microsoft\PowerApps-Targets</PowerAppsTargetsPath>
  <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
</PropertyGroup>
```

This creates a `.nupkg` file that bundles all dependencies for deployment to Dynamics 365 Online.

> **Note:** For **on-premises** deployments, you can register assemblies individually, but online requires one of the above approaches.

### Logging and Diagnostics

```csharp
public class MyPlugin : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
        var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

        // Log entity snapshot for debugging
        var preImage = context.PreEntityImages["PreImage"];
        var json = EntitySerializer.Serialize(preImage, indented: true);
        tracingService.Trace($"Pre-Image:\n{json}");
    }
}
```

### Azure Service Bus Integration

```csharp
public class SendToServiceBusPlugin : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
        var entity = (Entity)context.InputParameters["Target"];

        // Serialize and send to Azure Service Bus
        var json = EntitySerializer.Serialize(entity);
        var message = new ServiceBusMessage(json);

        // Send to your service bus (async pattern)
        await serviceBusClient.SendMessageAsync(message);
    }
}
```

### Webhook Notifications

```csharp
public class WebhookPlugin : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
        var entity = (Entity)context.InputParameters["Target"];

        // Send entity data to external webhook
        var json = EntitySerializer.Serialize(entity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using (var client = new HttpClient())
        {
            var response = client.PostAsync("https://your-webhook.com/api/updates", content).Result;
        }
    }
}
```

### FetchXML with Linked Entities

**AliasedValue support is critical for FetchXML queries with linked entities:**

```csharp
public class FetchXmlPlugin : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
        var service = serviceFactory.CreateOrganizationService(context.UserId);

        var fetchXml = @"
            <fetch>
              <entity name='account'>
                <attribute name='name' />
                <link-entity name='contact' from='contactid' to='primarycontactid' alias='contact'>
                  <attribute name='fullname' />
                  <attribute name='emailaddress1' />
                </link-entity>
              </entity>
            </fetch>";

        var results = service.RetrieveMultiple(new FetchExpression(fetchXml));

        // Serialize results including aliased values from linked entities
        var json = EntitySerializer.Serialize(results, indented: true);

        // Process each entity with aliased values
        foreach (var entity in results.Entities)
        {
            if (entity.Contains("contact.fullname"))
            {
                var aliasedValue = (AliasedValue)entity["contact.fullname"];
                var contactName = aliasedValue.Value.ToString();
            }
        }
    }
}
```

### Redis Caching in Plugins

```csharp
public class CachePlugin : IPlugin
{
    public void Execute(IServiceProvider serviceProvider)
    {
        var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
        var entityId = context.PrimaryEntityId;

        // Cache entity in Redis
        var cacheKey = $"account:{entityId}";
        var entity = /* retrieve entity */;
        var json = EntitySerializer.Serialize(entity);

        cache.SetString(cacheKey, json, TimeSpan.FromMinutes(15));

        // Later retrieve from cache
        var cachedJson = cache.GetString(cacheKey);
        if (cachedJson != null)
        {
            var cachedEntity = EntitySerializer.DeserializeEntity(cachedJson);
        }
    }
}
```

## External Integration Usage

For **external applications** (console apps, web APIs, Azure Functions), no special deployment is needed - just reference the NuGet package:

### Console Application Example

```csharp
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Xrm.Json.Serialization;

class Program
{
    static void Main(string[] args)
    {
        // Connect to Dynamics 365
        var connectionString = "AuthType=OAuth;...";
        var service = new CrmServiceClient(connectionString);

        // Retrieve and serialize
        var account = service.Retrieve("account", accountId, new ColumnSet(true));
        var json = EntitySerializer.Serialize(account, indented: true);

        Console.WriteLine(json);

        // Save to file, send to API, cache, etc.
        File.WriteAllText("account.json", json);
    }
}
```

### Azure Functions Example

```csharp
[FunctionName("ProcessDynamicsData")]
public static async Task<IActionResult> Run(
    [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
    ILogger log)
{
    // Deserialize from request
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    var entity = EntitySerializer.DeserializeEntity(requestBody);

    log.LogInformation($"Processing entity: {entity.LogicalName}");

    // Process and return
    var response = new 
    { 
        success = true,
        entityName = entity.LogicalName
    };

    return new OkObjectResult(response);
}
```

### Web API Example

```csharp
[ApiController]
[Route("api/[controller]")]
public class DynamicsController : ControllerBase
{
    private readonly IOrganizationService _service;

    [HttpGet("account/{id}")]
    public IActionResult GetAccount(Guid id)
    {
        var account = _service.Retrieve("account", id, new ColumnSet(true));
        var json = EntitySerializer.Serialize(account);

        return Content(json, "application/json");
    }

    [HttpPost("account")]
    public IActionResult CreateAccount([FromBody] string json)
    {
        var account = EntitySerializer.DeserializeEntity(json);
        var id = _service.Create(account);

        return Ok(new { id });
    }
}
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

### OptionSetValueCollection (Multi-Select Picklist)
```json
{
  "_options": [1, 2, 3]
}
```

### AliasedValue (FetchXML Linked Entities)
```json
{
  "_aliased": "contact|fullname|John Doe"
}
```

### BooleanManagedProperty
```json
{
  "_boolmanaged": "True|False"
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
  "createdon": "2024-01-15T10:30:00Z",
  "contact.fullname": {
    "_aliased": "contact|fullname|John Doe"
  }
}
```

## Requirements

- **.NET Framework 4.6.2** or higher (**Plugin compatible!** ✅)
- **Microsoft.CrmSdk.CoreAssemblies** (>= 9.0.2.60)
- **Newtonsoft.Json** (>= 13.0.3)

> **Note:** This library targets .NET Framework 4.6.2, making it fully compatible with Dynamics 365 plugins which run on .NET Framework 4.6.2 runtime.

## Use Cases

This library is ideal for:

- **Plugin development** - Logging, diagnostics, and external integrations in Dynamics 365 plugins
- **FetchXML queries** - Serialize results with linked entities (AliasedValue support)
- **API integrations** - Transmit Dynamics 365 data via REST APIs
- **Data export/import** - Backup and restore entity data
- **Caching** - Store entity data in Redis, file systems, or other caches
- **Logging** - Serialize entities for audit trails and debugging
- **Message queues** - Send entity data through Azure Service Bus, RabbitMQ, etc.
- **Webhooks** - Push Dynamics 365 changes to external systems
- **Multi-select picklists** - Handle OptionSetValueCollection fields

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

### Version 1.2026.3.0 (March 2026)
- ✅ **NEW:** AliasedValue converter for FetchXML linked entity support
- ✅ **NEW:** OptionSetValueCollection converter for multi-select picklists
- ✅ **NEW:** BooleanManagedProperty converter
- ✅ **NEW:** EntitySerializer helper class for simplified usage
- ✅ **FIX:** String escaping issue with double quotes and special characters (#20)
- ✅ **TARGET:** .NET Framework 4.6.2 (plugin compatible)
- ✅ **DOCS:** Added comprehensive plugin usage examples
- ✅ **TESTS:** Added 27 new tests for new converters

### Version 1.2022.10.1 (October 2022)
- Namespace change from Innofactor.Xrm.Json.Serialization to Xrm.Json.Serialization
- Package metadata updates

### Version 1.0.0 (2021)
- Initial release
- Support for all major Dynamics 365 data types
- Entity, EntityReference, EntityCollection converters
- OptionSetValue, Money, DateTime, Guid converters
- XrmContractResolver for seamless integration
- Comprehensive test coverage
