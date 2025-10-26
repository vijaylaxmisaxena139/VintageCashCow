using NUnit.Framework;
using ProductPricing.Services;
using ProductPricing.Models;
using System.Linq;
using System;

namespace ProductPricing.Tests;

public class ProductServiceTests
{
    private ProductService _service = null!;

    [SetUp]
    public void Setup()
    {
        _service = new ProductService();
    }

    [Test]
    public void GetProducts_ReturnsInitialProducts()
    {
        var products = _service.GetProducts().ToList();
        Assert.That(products.Count, Is.GreaterThanOrEqualTo(2));
        var p1 = products.FirstOrDefault(p => p.Id == 1);
        Assert.IsNotNull(p1);
        Assert.AreEqual("Product A", p1!.Name);
    }

    [Test]
    public void ApplyDiscount_DecreasesPriceAndAddsHistoryEntry()
    {
        var before = _service.GetProducts().First(p => p.Id == 1);
        var originalPrice = before.Price;

        var updated = _service.ApplyDiscount(1, 10m);

        Assert.IsNotNull(updated);
        Assert.Less(updated.Price, originalPrice);
        Assert.AreEqual(Math.Round(originalPrice * 0.9m, 2), updated.Price);

        var history = _service.GetPriceHistory(1);
        Assert.IsNotNull(history);
        Assert.IsTrue(history.PriceHistory.Count >= 3); // initial entries plus new
        Assert.AreEqual(originalPrice, history.PriceHistory[0].Price);
    }

    [Test]
    public void UpdatePrice_ChangesPriceAndAddsHistoryEntry()
    {
        var before = _service.GetProducts().First(p => p.Id == 2);
        var originalPrice = before.Price;

        var updated = _service.UpdatePrice(2, 150m);

        Assert.IsNotNull(updated);
        Assert.AreEqual(150m, updated.Price);

        var history = _service.GetPriceHistory(2);
        Assert.IsNotNull(history);
        Assert.IsTrue(history.PriceHistory.Count >= 1);
        Assert.AreEqual(originalPrice, history.PriceHistory[0].Price);
    }

    [Test]
    public void ApplyDiscount_InvalidProduct_ReturnsNull()
    {
        var updated = _service.ApplyDiscount(999, 10m);
        Assert.IsNull(updated);
    }

    [Test]
    public void UpdatePrice_InvalidProduct_ReturnsNull()
    {
        var updated = _service.UpdatePrice(999, 10m);
        Assert.IsNull(updated);
    }
}
