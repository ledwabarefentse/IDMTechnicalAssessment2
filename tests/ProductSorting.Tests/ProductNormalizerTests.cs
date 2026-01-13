using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductSorting.Models;
using ProductSorting.Services;

namespace ProductSorting.Tests;

[TestClass]
public class ProductNormalizerTests
{
    /// <summary>Extracts a trailing 1-2 letter category from the name when missing.</summary>
    [TestMethod]
    public void NormalizeCategoryFromName_SplitsTrailingToken()
    {
        var products = new List<Product>
        {
            new("Widget AA", null, 10.00m, 5),
            new("Gadget B", null, 12.00m, 2),
        };

        var normalized = ProductNormalizer.NormalizeCategoryFromName(products);

        Assert.AreEqual("Widget", normalized[0].Name);
        Assert.AreEqual("AA", normalized[0].Category);
        Assert.AreEqual("Gadget", normalized[1].Name);
        Assert.AreEqual("B", normalized[1].Category);
    }

    /// <summary>Leaves products untouched when category is already present.</summary>
    [TestMethod]
    public void NormalizeCategoryFromName_SkipsWhenCategoryPresent()
    {
        var products = new List<Product>
        {
            new("Widget", "A", 10.00m, 5),
        };

        var normalized = ProductNormalizer.NormalizeCategoryFromName(products);

        Assert.AreEqual("Widget", normalized[0].Name);
        Assert.AreEqual("A", normalized[0].Category);
    }
}
