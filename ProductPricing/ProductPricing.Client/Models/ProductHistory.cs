namespace ProductPricing.Models;

public class ProductHistory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<PriceEntry> PriceHistory { get; set; } = new();
}
