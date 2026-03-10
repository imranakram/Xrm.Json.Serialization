# Fix Project Structure Script
# Run this script AFTER closing Visual Studio

Write-Host "=== Xrm.Json.Serialization Structure Fix ===" -ForegroundColor Cyan
Write-Host ""

$root = "C:\Git\GitHub\Xrm.Json.Serialization"
Set-Location $root

# Step 1: Move projects
Write-Host "Step 1: Moving projects to root..." -ForegroundColor Yellow
if (Test-Path "src\Innofactor.Xrm.Json.Serialization") {
    Move-Item -Path "src\Innofactor.Xrm.Json.Serialization" -Destination "." -Force
    Write-Host "  Moved main project" -ForegroundColor Green
}

if (Test-Path "src\Innofactor.Xrm.Json.Serialization.Tests") {
    Move-Item -Path "src\Innofactor.Xrm.Json.Serialization.Tests" -Destination "." -Force
    Write-Host "  Moved test project" -ForegroundColor Green
}

if (Test-Path "src") {
    Remove-Item -Path "src" -Recurse -Force
    Write-Host "  Removed src folder" -ForegroundColor Green
}

# Step 2: Update solution file
Write-Host ""
Write-Host "Step 2: Updating solution file..." -ForegroundColor Yellow
$solutionFile = "Xrm.Json.Serialization.sln"
$content = Get-Content $solutionFile -Raw
$content = $content -replace 'src\\Innofactor\.Xrm\.Json\.Serialization\\', 'Innofactor.Xrm.Json.Serialization\'
$content = $content -replace 'src\\Innofactor\.Xrm\.Json\.Serialization\.Tests\\', 'Innofactor.Xrm.Json.Serialization.Tests\'
Set-Content $solutionFile -Value $content -NoNewline
Write-Host "  Updated solution paths" -ForegroundColor Green

# Step 3: Update project file paths
Write-Host ""
Write-Host "Step 3: Updating NuGet package paths in project files..." -ForegroundColor Yellow

$mainProj = "Innofactor.Xrm.Json.Serialization\Innofactor.Xrm.Json.Serialization.csproj"
if (Test-Path $mainProj) {
    $content = Get-Content $mainProj -Raw
    $content = $content -replace '\.\.\\\.\.\\packages\\', '..\packages\'
    Set-Content $mainProj -Value $content -NoNewline
    Write-Host "  Updated main project" -ForegroundColor Green
}

$testProj = "Innofactor.Xrm.Json.Serialization.Tests\Innofactor.Xrm.Json.Serialization.Tests.csproj"
if (Test-Path $testProj) {
    $content = Get-Content $testProj -Raw
    $content = $content -replace '\.\.\\\.\.\\packages\\', '..\packages\'
    Set-Content $testProj -Value $content -NoNewline
    Write-Host "  Updated test project" -ForegroundColor Green
}

# Step 4: Clean build artifacts
Write-Host ""
Write-Host "Step 4: Cleaning build artifacts..." -ForegroundColor Yellow
Remove-Item -Recurse -Force "Innofactor.Xrm.Json.Serialization\bin", "Innofactor.Xrm.Json.Serialization\obj" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "Innofactor.Xrm.Json.Serialization.Tests\bin", "Innofactor.Xrm.Json.Serialization.Tests\obj" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force ".vs" -ErrorAction SilentlyContinue
Write-Host "  Cleaned bin/obj folders" -ForegroundColor Green

# Step 5: Restore NuGet packages
Write-Host ""
Write-Host "Step 5: Restoring NuGet packages..." -ForegroundColor Yellow
nuget restore Xrm.Json.Serialization.sln
Write-Host "  Packages restored" -ForegroundColor Green

Write-Host ""
Write-Host "=== Structure Fix Complete! ===" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "  1. Open Xrm.Json.Serialization.sln in Visual Studio" -ForegroundColor White
Write-Host "  2. Build -> Rebuild Solution" -ForegroundColor White
Write-Host "  3. Test -> Run All Tests" -ForegroundColor White
Write-Host ""
