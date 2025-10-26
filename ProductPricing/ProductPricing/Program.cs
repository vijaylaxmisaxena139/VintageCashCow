using ProductPricing.Client.Pages;
using ProductPricing.Components;
using ProductPricing.Services;
using System.Net.Http;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// Add CORS for development (permissive)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register HttpClient for server-side DI so components (when activated on the server) can use HttpClient
// Try to read the URLs configured, otherwise fall back to the common development HTTP URL used by this solution.
var urls = builder.Configuration["ASPNETCORE_URLS"];
var firstUrl = urls?.Split(';').FirstOrDefault();
var serverBase = firstUrl ?? "http://localhost:5220"; // fallback to the http profile port
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(serverBase) });

// Add API controllers and product service
builder.Services.AddControllers();
builder.Services.AddSingleton<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

// Enable CORS (development)
app.UseCors("DevCors");

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ProductPricing.Client._Imports).Assembly);

// Map API controllers
app.MapControllers();

app.Run();
