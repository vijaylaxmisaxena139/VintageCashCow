# VintageCashCow — ProductPricing (Coding Test)

This repository contains the ProductPricing coding test submitted by Vijaylaxmi Saxena for VintageCashCow.

The solution demonstrates a small Blazor WebAssembly + ASP.NET Core application that manages product prices and price history. It includes a server host with API endpoints, a Blazor WebAssembly client, and unit tests for the service logic.

Summary
- Candidate: Vijaylaxmi Saxena
- Assignment: ProductPricing coding test for VintageCashCow
- Technology: .NET 9, Blazor WebAssembly, ASP.NET Core, NUnit

Repository layout
- `ProductPricing/` — ASP.NET Core host project with API controllers and services
- `ProductPricing.Client/` — Blazor WebAssembly client (interactive render mode)
- `ProductPricing.Tests/` — Unit tests for server-side services

Prerequisites
- .NET 9 SDK
- Git
- Optional: Visual Studio or VS Code with C# tooling

Quick start
1. Build the solution:
   ```bash
   dotnet build
   ```

2. Run the host (from repo root):
   ```bash
   dotnet run --project ProductPricing\ProductPricing.csproj
   ```
   Default fallback URL: `http://localhost:5220` (see `ProductPricing/Program.cs`).

3. Open the UI in your browser:
   - `http://localhost:5220` — host root
   - `http://localhost:5220/products` — products page

Run tests
```bash
dotnet test
```

Notes and suggestions
- The server uses an in-memory `ProductService`. For production, replace it with a persistent store (EF Core, etc.).
- Consider adding a `ProductPricing.Shared` project for shared DTOs/models used by client and server.
- CI: add GitHub Actions (or other) to run `dotnet build` and `dotnet test` on PRs.

Contact / author
- Candidate: Vijaylaxmi Saxena

- Repository origin: https://github.com/vijaylaxmisaxena139/VintageCashCow
