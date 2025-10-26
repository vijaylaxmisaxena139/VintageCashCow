# Run development server for hosted Blazor WASM + API
# Usage: Open PowerShell in repository root and run: .\run-dev.ps1

$ErrorActionPreference = 'Stop'

Write-Host "Building Blazor WebAssembly client..."
dotnet build "ProductPricing.ProductPricing.Client/ProductPricing.Client.csproj" -c Debug

Write-Host "Running hosted server (will serve the client)" -ForegroundColor Green
dotnet run --project "ProductPricing/ProductPricing.csproj"
