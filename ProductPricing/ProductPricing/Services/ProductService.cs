using ProductPricing.Models;

namespace ProductPricing.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products = new()
    {
        new Product{ Id = 1, Name = "Product A", Price = 100m, LastUpdated = DateTime.Parse("2024-09-26T12:34:56") },
        new Product{ Id = 2, Name = "Product B", Price = 200m, LastUpdated = DateTime.Parse("2024-09-25T10:12:34") }
    };

    private readonly Dictionary<int, List<PriceEntry>> _history = new()
    {
        {1, new List<PriceEntry>
            {
                new PriceEntry{ Price = 120m, Date = DateOnly.Parse("2024-09-01") },
                new PriceEntry{ Price = 110m, Date = DateOnly.Parse("2024-08-15") },
                new PriceEntry{ Price = 100m, Date = DateOnly.Parse("2024-08-10") }
            }
        },
        {2, new List<PriceEntry>
            {
                new PriceEntry{ Price = 200m, Date = DateOnly.Parse("2024-09-01") }
            }
        }
    };

    public IEnumerable<Product> GetProducts()
    {
        return _products;
    }

    public ProductHistory GetPriceHistory(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null) return null!;
        _history.TryGetValue(id, out var entries);
        return new ProductHistory
        {
            Id = product.Id,
            Name = product.Name,
            PriceHistory = entries ?? new List<PriceEntry>()
        };
    }

    public Product ApplyDiscount(int id, decimal discountPercentage)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null) return null!;
        var originalPrice = product.Price;
        var discounted = Math.Round(originalPrice * (1 - discountPercentage / 100m), 2);

        // Record history
        if (!_history.ContainsKey(id)) _history[id] = new List<PriceEntry>();
        _history[id].Insert(0, new PriceEntry { Price = originalPrice, Date = DateOnly.FromDateTime(DateTime.UtcNow) });

        product.Price = discounted;
        product.LastUpdated = DateTime.UtcNow;

        return new Product
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            LastUpdated = product.LastUpdated
        };
    }

    public Product UpdatePrice(int id, decimal newPrice)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null) return null!;

        // Record history
        if (!_history.ContainsKey(id)) _history[id] = new List<PriceEntry>();
        _history[id].Insert(0, new PriceEntry { Price = product.Price, Date = DateOnly.FromDateTime(DateTime.UtcNow) });

        product.Price = newPrice;
        product.LastUpdated = DateTime.UtcNow;

        return product;
    }
}
