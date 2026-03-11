# GitHub Actions Setup Guide

This project uses **GitHub Actions** for CI/CD instead of AppVeyor.

## 🔧 Setup Instructions

### 1. Update Git Remote (Already Done)
The repository was moved from `Biznamics` to `imranakram`:

```bash
git remote set-url origin https://github.com/imranakram/Xrm.Json.Serialization
git remote -v
```

### 2. Enable GitHub Actions
GitHub Actions is enabled by default for public repositories. The workflows are already configured in `.github/workflows/`.

### 3. Configure NuGet API Key Secret

To enable automatic NuGet publishing on release:

1. Go to https://www.nuget.org/account/apikeys
2. Create a new API key with:
   - **Key Name:** `GitHub-Actions-Xrm.Json.Serialization`
   - **Glob Pattern:** `Xrm.Json.Serialization`
   - **Scopes:** Push new packages and package versions
3. Copy the generated API key
4. Go to your GitHub repository → **Settings** → **Secrets and variables** → **Actions**
5. Click **New repository secret**
6. Name: `NUGET_API_KEY`
7. Value: Paste your NuGet API key
8. Click **Add secret**

### 4. Workflows

#### **Build and Test** (`.github/workflows/build-and-test.yml`)
- **Triggers:** Push to `main`, `vNext`, `develop` branches; Pull requests
- **Actions:**
  - Restore NuGet packages
  - Build solution in Release mode
  - Run all unit tests
  - Upload test results and build artifacts

#### **Publish to NuGet** (`.github/workflows/publish-nuget.yml`)
- **Triggers:** Push tags matching `v*.*.*` (e.g., `v1.2026.3.0`)
- **Actions:**
  - Extract version from Git tag
  - Build and test
  - Update .nuspec with version from tag
  - Pack NuGet package
  - Publish to NuGet.org
  - Create GitHub Release with package attached

### 5. Publishing a Release

When you're ready to publish version **1.2026.3.0** (CalVer format):

```bash
# Commit all changes
git add .
git commit -m "Release v1.2026.3.0 - Add AliasedValue support and fix #20"

# Create and push tag (CalVer: 1.YYYY.MM.patch)
git tag v1.2026.3.0
git push origin vNext
git push origin v1.2026.3.0
```

This will automatically:
1. Trigger the publish workflow
2. Extract version from tag (v1.2026.3.0 → 1.2026.3.0)
3. Update .nuspec version dynamically
4. Build and test the solution
5. Create the NuGet package
6. Push to NuGet.org
7. Create GitHub Release with artifacts

### 6. Monitoring

- View workflow runs: https://github.com/imranakram/Xrm.Json.Serialization/actions
- Check build status badge in README.md
- NuGet publish status will appear in the workflow logs

## 🗑️ AppVeyor Migration

The old `appveyor.yml` has been marked as deprecated and kept for reference only. You can:

**Option 1:** Delete it entirely
```bash
git rm appveyor.yml
git commit -m "Remove deprecated AppVeyor configuration"
```

**Option 2:** Keep it for reference (current state)

If you decide to keep using AppVeyor instead:
1. Update the AppVeyor project settings at https://ci.appveyor.com
2. The configuration has been updated to work with the new structure
3. Update the NuGet API key in AppVeyor settings

## 📊 Benefits of GitHub Actions

| Feature | GitHub Actions | AppVeyor |
|---------|---------------|----------|
| **Cost** | Free (public repos) | Free tier limited |
| **Integration** | Native GitHub | External service |
| **Setup** | No account needed | Separate account |
| **Marketplace** | 10,000+ actions | Limited |
| **Build Minutes** | 2,000/month free | 1 concurrent job |
| **Artifacts** | 500MB storage | Limited |
| **Modern** | Active development | Legacy |

## 🎯 Recommended Next Steps

1. ✅ **Update Git remote:** `git remote set-url origin https://github.com/imranakram/Xrm.Json.Serialization`
2. ✅ **Manually update version numbers** in `.nuspec` and `CHANGELOG.md` to use **CalVer format** (`1.2026.3.0`)
3. ✅ Test the GitHub Actions workflows by pushing a commit to vNext branch
4. ✅ Add NuGet API key secret to GitHub (see step 3 above)
5. ✅ When ready, create release tag: `git tag v1.2026.3.0 && git push origin v1.2026.3.0`
6. ✅ Monitor workflow run at: https://github.com/imranakram/Xrm.Json.Serialization/actions

## 📝 Notes

- GitHub Actions builds on Windows (required for .NET Framework 4.6.2)
- Test results are preserved as artifacts
- NuGet packages are created on every tag push
- Build status badge is already added to README.md
- **Version format:** CalVer `1.YYYY.MM.patch` (e.g., `1.2026.3.0` for March 2026)
- Git tag format: `v1.2026.3.0` (with 'v' prefix)
- The workflow automatically extracts version from tag and updates .nuspec
