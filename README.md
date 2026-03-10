# Xrm.Json.Serialization

Compact JSON serialization for Microsoft Dynamics 365/CRM/Dataverse entities using Newtonsoft.Json.

## Features

- Full support for Entity, EntityReference, EntityCollection
- OptionSetValue, Money, DateTime, Guid converters
- CRM-optimized type handling
- Built for .NET Framework 4.8 with Dynamics 365 SDK 9.x

## Installation

```powershell
Install-Package Xrm.Json.Serialization
``` 

## Quick Start

```csharp
using Xrm.Json.Serialization;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;

var entity = new Entity(\"account\", Guid.NewGuid());
entity[\"name\"] = \"Contoso\";
string json = JsonConvert.SerializeObject(entity, new EntityConverter());
```
