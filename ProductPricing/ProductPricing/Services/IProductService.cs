using ProductPricing.Models;

namespace ProductPricing.Services;

public interface IProductService
{
    IEnumerable<Product> GetProducts();
    ProductHistory? GetPriceHistory(int id);
    Product? ApplyDiscount(int id, decimal discountPercentage);
    Product? UpdatePrice(int id, decimal newPrice);
}
