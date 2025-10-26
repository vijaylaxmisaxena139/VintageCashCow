using Microsoft.AspNetCore.Mvc;
using ProductPricing.Models;
using ProductPricing.Services;
using System.ComponentModel.DataAnnotations;

namespace ProductPricing.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDto>> Get()
    {
        var products = _service.GetProducts().Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            LastUpdated = p.LastUpdated
        });
        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<ProductHistory> Get(int id)
    {
        var history = _service.GetPriceHistory(id);
        if (history == null) return NotFound();
        return Ok(history);
    }

    public class DiscountRequest { [Required] [Range(0, 100)] public decimal DiscountPercentage { get; set; } }

    public class ApplyDiscountResult { public int Id { get; set; } public string Name { get; set; } = string.Empty; public decimal OriginalPrice { get; set; } public decimal DiscountedPrice { get; set; } }

    [HttpPost("{id}/apply-discount")]
    public ActionResult<ApplyDiscountResult> ApplyDiscount(int id, [FromBody] DiscountRequest req)
    {
        if (req == null) return BadRequest("Request body is required.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var productBefore = _service.GetProducts().FirstOrDefault(p => p.Id == id);
        if (productBefore == null) return NotFound();
        var originalPrice = productBefore.Price;

        var updated = _service.ApplyDiscount(id, req.DiscountPercentage);
        if (updated == null) return NotFound();
        return Ok(new ApplyDiscountResult
        {
            Id = updated.Id,
            Name = updated.Name,
            OriginalPrice = Math.Round(originalPrice, 2),
            DiscountedPrice = updated.Price
        });
    }

    public class UpdatePriceRequest { [Required] [Range(0.01, double.MaxValue)] public decimal NewPrice { get; set; } }
    public class UpdatePriceResult { public int Id { get; set; } public string Name { get; set; } = string.Empty; public decimal NewPrice { get; set; } public DateTime LastUpdated { get; set; } }

    [HttpPut("{id}/update-price")]
    public ActionResult<UpdatePriceResult> UpdatePrice(int id, [FromBody] UpdatePriceRequest req)
    {
        if (req == null) return BadRequest("Request body is required.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = _service.UpdatePrice(id, req.NewPrice);
        if (updated == null) return NotFound();
        return Ok(new UpdatePriceResult
        {
            Id = updated.Id,
            Name = updated.Name,
            NewPrice = updated.Price,
            LastUpdated = updated.LastUpdated
        });
    }

    public sealed class ProductDto { public int Id { get; set; } public string Name { get; set; } = string.Empty; public decimal Price { get; set; } public DateTime LastUpdated { get; set; } }
}
