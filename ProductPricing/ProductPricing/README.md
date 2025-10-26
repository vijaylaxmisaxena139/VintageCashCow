# ProductPricing

A small example Blazor WebAssembly + ASP.NET Core solution demonstrating a basic product pricing UI and an API for managing prices and viewing price history.

This README is a friendly guide to help you get the solution running locally, explain the main pieces of the project, and show how to work with the code.

---

## Quick overview

- `ProductPricing` — ASP.NET Core host with API controllers and server-side glue for interactive WebAssembly rendering.
- `ProductPricing.Client` — Blazor WebAssembly components and pages (interactive render mode) that call the API.
- `ProductPricing.Tests` — Unit tests (NUnit) for server-side services.

The sample app stores products in memory (in `ProductService`) and exposes simple endpoints to list products, view price history, apply a discount, and update a product's price.

---

## Requirements

- .NET 9 SDK
- Git (for cloning) and a terminal
- Recommended: Visual Studio 2022/2023 or VS Code with the C# extension

---

## Quick start (run locally)

1. Restore and build the solution:

   ```bash
   dotnet build
   ```

2. Run the server (host) project. From the repository root run either:

   - using `dotnet run` directly:
     ```bash
     dotnet run --project ProductPricing\ProductPricing.csproj
     ```

   - or use the included `run-dev.ps1` PowerShell helper (on Windows/PowerShell):
     ```powershell
     .\run-dev.ps1
     ```

   By default the host will pick a URL from the `ASPNETCORE_URLS` environment variable if present; otherwise it falls back to `http://localhost:5220` (see `Program.cs`).

3. Open your browser and go to:

   - Host root (Blazor host): `http://localhost:5220` (or the URL printed by `dotnet run`)
   - Products page: `http://localhost:5220/products`

The `/products` page shows the product list and simple controls to apply discounts or update prices.

---

## API endpoints (examples)

The host exposes a few JSON API endpoints used by the client UI. Example `curl` commands below assume `http://localhost:5220` as the server base URL.

- List products

  GET `/api/products`

  ```bash
  curl http://localhost:5220/api/products
  ```

  Response: an array of products with `id`, `name`, `price`, `lastUpdated`.

- Get product price history

  GET `/api/products/{id}`

  ```bash
  curl http://localhost:5220/api/products/1
  ```

  Response: `ProductHistory` object containing `PriceHistory` entries (price + date).

- Apply discount

  POST `/api/products/{id}/apply-discount`

  Body (JSON):
  ```json
  { "DiscountPercentage": 10 }
  ```

  Example:
  ```bash
  curl -X POST -H "Content-Type: application/json" -d "{\"DiscountPercentage\":10}" http://localhost:5220/api/products/1/apply-discount
  ```

  Response: JSON with `Id`, `Name`, `OriginalPrice`, `DiscountedPrice`.

- Update price

  PUT `/api/products/{id}/update-price`

  Body (JSON):
  ```json
  { "NewPrice": 123.45 }
  ```

  Example:
  ```bash
  curl -X PUT -H "Content-Type: application/json" -d "{\"NewPrice\":123.45}" http://localhost:5220/api/products/1/update-price
  ```

  Response: JSON with `Id`, `Name`, `NewPrice`, `LastUpdated`.

---

## Client UI

- Page: `ProductPricing.Client/Pages/ProductList.razor`
  - Lists products fetched from the API
  - For each product you can:
    - Click "View History" to open the `ProductHistoryView` component
    - Enter a discount percentage and click Apply
    - Enter a new price and click Update

Notes about the client:
- The client uses simple `<input type="number">` fields with `@bind` to local dictionaries keyed by product id.
- JSON properties returned by the API use PascalCase, and client models in `ProductPricing.Client/Models` match the server models.

---

## Tests

Unit tests are in the `ProductPricing.Tests` project (NUnit). Run them with:

```bash
dotnet test
```

You should see the tests build and execute; the sample tests cover `ProductService` behavior (apply discount, update price, and history bookkeeping).

Note: you may see a NuGet warning that a slightly different NUnit minor version was resolved — this is typically harmless. You can update the test project's package versions to remove the warning.

---

## Project layout (important files)

- `ProductPricing/Program.cs` — host startup, DI registration, and static assets mapping
- `ProductPricing/Controllers/ProductsController.cs` — API endpoints
- `ProductPricing/Services/ProductService.cs` — in-memory product storage and operations
- `ProductPricing/Models` — domain models: `Product`, `ProductHistory`, `PriceEntry`, `ProductDto`
- `ProductPricing.Client/Pages/ProductList.razor` — main Blazor page for product actions
- `ProductPricing.Client/Pages/ProductHistoryView.razor` — component that renders history
- `ProductPricing.Tests/ProductServiceTests.cs` — unit tests

---

## Recommended improvements / next steps

If you plan to extend this example for development or production, consider:

- Moving shared DTOs and models into a `ProductPricing.Shared` class library referenced by both server and client to avoid duplicated definitions and accidental mismatches.
- Replacing the in-memory `ProductService` with a repository backed by a real database (EF Core or other) and add migrations.
- Adding validation and better client-side input feedback (e.g., show validation messages inline).
- Adding authentication/authorization if pricing operations should be restricted.
- Adding end-to-end or UI tests (Playwright or Selenium) for the Blazor client.

---

## Troubleshooting

- If the client can't reach the API:
  - Make sure the host is running and you are using the correct base URL.
  - Check CORS settings (the sample includes a permissive `DevCors` policy in `Program.cs` for development).

- If you get JSON deserialization errors on the client, verify that the server response property names match the client models.

- If you see a NuGet package resolution warning for NUnit when running tests, update test package versions in `ProductPricing.Tests.csproj` or ignore if tests run successfully.

---

