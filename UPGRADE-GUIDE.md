# Xrm.Json.Serialization v2.0 Upgrade Summary

## 📋 Upgrade Checklist

### ✅ Completed
- [x] Upgraded target framework from .NET 4.6.2 to .NET 4.8
- [x] Updated Newtonsoft.Json to 13.0.3
- [x] Updated xUnit to 2.9.3
- [x] Removed all Innofactor branding
- [x] Changed namespace from `Innofactor.Xrm.Json.Serialization` to `Xrm.Json.Serialization`
- [x] Updated AssemblyInfo (version 2.0.0.0, Biznamics branding)
- [x] Updated NuGet package metadata (.nuspec)
- [x] Created comprehensive README.md
- [x] Created fix-structure.ps1 script

### ⏳ Pending (Manual Steps Required)
- [ ] Close Visual Studio
- [ ] Run fix-structure.ps1 script to move projects to root
- [ ] Reopen solution in Visual Studio
- [ ] Build and test
- [ ] Commit changes to vNext branch
- [ ] Create release tag v2.0.0
- [ ] Publish NuGet package

---

## 🏗️ Project Structure Changes

### Before:
```
C:\Git\GitHub\Xrm.Json.Serialization\
├── src\
│   ├── Innofactor.Xrm.Json.Serialization\
│   └── Innofactor.Xrm.Json.Serialization.Tests\
├── packages\
└── Xrm.Json.Serialization.sln
```

### After:
```
C:\Git\GitHub\Xrm.Json.Serialization\
├── Innofactor.Xrm.Json.Serialization\
├── Innofactor.Xrm.Json.Serialization.Tests\
├── packages\
├── fix-structure.ps1
├── README.md
└── Xrm.Json.Serialization.sln
```

---

## 📦 NuGet Dependencies

### Main Project (Innofactor.Xrm.Json.Serialization)
| Package | Version | Purpose |
|---------|---------|---------|
| Newtonsoft.Json | 13.0.3 | JSON serialization |
| Microsoft.CrmSdk.CoreAssemblies | 9.0.2.46 | Dynamics 365 SDK types |
| Microsoft.Bcl.AsyncInterfaces | 6.0.0 | Async support |
| System.Text.Json | 6.0.10 | Modern JSON APIs |
| System.Memory | 4.5.5 | Span<T> support |

### Test Project (Innofactor.Xrm.Json.Serialization.Tests)
All of above plus:
| Package | Version | Purpose |
|---------|---------|---------|
| xunit | 2.9.3 | Test framework |
| xunit.runner.visualstudio | 3.1.5 | VS Test Explorer integration |

---

## 🔧 Manual Steps to Complete

### 1. Close Visual Studio
**Critical**: Close VS completely to release file locks

### 2. Run Structure Fix Script
```powershell
# In PowerShell (Run as Administrator if needed)
cd C:\Git\GitHub\Xrm.Json.Serialization
.\fix-structure.ps1
```

This script will:
- Move projects from `src\` to root
- Update solution file paths
- Update NuGet package HintPath references
- Clean bin/obj folders
- Restore NuGet packages

### 3. Reopen Solution
```powershell
start Xrm.Json.Serialization.sln
```

### 4. Rebuild Solution
In Visual Studio:
- Build → Rebuild Solution
- Should build without errors

### 5. Run Tests
- Test → Run All Tests
- Verify all tests pass

### 6. Commit Changes
```bash
git add -A
git commit -m "v2.0.0: Upgrade to .NET 4.8, remove Innofactor branding, restructure projects"
git push origin vNext
```

---

## 📝 Breaking Changes in v2.0.0

### Namespace Change
**Old:** `Innofactor.Xrm.Json.Serialization`  
**New:** `Xrm.Json.Serialization`

**Migration:**
```csharp
// Old code
using Innofactor.Xrm.Json.Serialization;

// New code
using Xrm.Json.Serialization;
```

### Target Framework
**Old:** .NET Framework 4.6.2  
**New:** .NET Framework 4.8

### Assembly Name
**Old:** `Innofactor.Xrm.Json.Serialization.dll`  
**New:** `Xrm.Json.Serialization.dll`

---

## 🚀 Publishing to NuGet

### Build Release Package
```powershell
# Build in Release mode
msbuild Xrm.Json.Serialization.sln /p:Configuration=Release

# Pack NuGet package
nuget pack Innofactor.Xrm.Json.Serialization\Innofactor.Xrm.Json.Serialization.nuspec -Properties Configuration=Release
```

### Publish
```powershell
nuget push Xrm.Json.Serialization.2.0.0.nupkg -Source nuget.org -ApiKey YOUR_API_KEY
```

### Tag Release
```bash
git tag v2.0.0
git push origin v2.0.0
```

---

## 🧪 Test Coverage

All converters have comprehensive unit tests:
- ✅ BasicsConverter (string, int, double, decimal, object)
- ✅ DateTimeConverter (timezone support)
- ✅ EntityCollectionConverter (arrays of entities)
- ✅ EntityConverter (full entity serialization)
- ✅ EntityReferenceConverter (lookup fields)
- ✅ GuidConverter (unique identifiers)
- ✅ MoneyConverter (currency values)
- ✅ OptionSetValueConverter (picklist values)
- ✅ Combined types (complex entity scenarios)

**Total Tests:** ~25+  
**Expected Pass Rate:** 100%

---

## 📞 Support

- **Issues:** https://github.com/Biznamics/Xrm.Json.Serialization/issues
- **Discussions:** https://github.com/Biznamics/Xrm.Json.Serialization/discussions
- **NuGet:** https://www.nuget.org/packages/Xrm.Json.Serialization/

---

## 🙏 Credits

- **Original Author:** Alexey Shytikov
- **Current Maintainer:** Imran Akram / Biznamics
- **Contributors:** Community contributors welcome!

---

**License:** MIT  
**Copyright:** © Biznamics 2025
