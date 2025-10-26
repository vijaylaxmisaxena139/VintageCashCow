using Microsoft.AspNetCore.Mvc;
using ProductPricing.Models;
using ProductPricing.Services;
using System.ComponentModel.DataAnnotations;

namespace ProductPricing.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<object>> Get()
    {
        var products = _service.GetProducts().Select(p => new
        {
            id = p.Id,
            name = p.Name,
            price = p.Price,
            lastUpdated = p.LastUpdated
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
    [HttpPost("{id}/apply-discount")]
    public ActionResult<object> ApplyDiscount(int id, [FromBody] DiscountRequest req)
    {
        if (req == null) return BadRequest("Request body is required.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Get current price before applying discount
        var productBefore = _service.GetProducts().FirstOrDefault(p => p.Id == id);
        if (productBefore == null) return NotFound();
        var originalPrice = productBefore.Price;

        var updated = _service.ApplyDiscount(id, req.DiscountPercentage);
        if (updated == null) return NotFound();
        return Ok(new
        {
            id = updated.Id,
            name = updated.Name,
            originalPrice = Math.Round(originalPrice, 2),
            discountedPrice = updated.Price
        });
    }

    public class UpdatePriceRequest { [Required] [Range(0, double.MaxValue)] public decimal NewPrice { get; set; } }
    [HttpPut("{id}/update-price")]
    public ActionResult<object> UpdatePrice(int id, [FromBody] UpdatePriceRequest req)
    {
        if (req == null) return BadRequest("Request body is required.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = _service.UpdatePrice(id, req.NewPrice);
        if (updated == null) return NotFound();
        return Ok(new
        {
            id = updated.Id,
            name = updated.Name,
            newPrice = updated.Price,
            lastUpdated = updated.LastUpdated
        });
    }
}
