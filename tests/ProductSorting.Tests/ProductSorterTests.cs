using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductSorting.Models;
using ProductSorting.Services;

namespace ProductSorting.Tests;

[TestClass]
public class ProductSorterTests
{
    /// <summary>Verifies price sorting uses name as a tie-breaker.</summary>
    [TestMethod]
    public void SortByPriceAscending_OrdersByPriceThenName()
    {
        var products = new List<Product>
        {
            new("Widget", null, 12.50m, 3),
            new("Alpha", null, 9.99m, 5),
            new("Beta", null, 9.99m, 2),
        };

        var sorted = ProductSorter.SortByPriceAscending(products);

        Assert.AreEqual("Alpha", sorted[0].Name);
        Assert.AreEqual("Beta", sorted[1].Name);
        Assert.AreEqual("Widget", sorted[2].Name);
    }

    /// <summary>Verifies quantity sorting uses name as a tie-breaker.</summary>
    [TestMethod]
    public void SortByQuantityAscending_OrdersByQuantityThenName()
    {
        var products = new List<Product>
        {
            new("Widget", null, 12.50m, 3),
            new("Alpha", null, 9.99m, 3),
            new("Beta", null, 9.99m, 2),
        };

        var sorted = ProductSorter.SortByQuantityAscending(products);

        Assert.AreEqual("Beta", sorted[0].Name);
        Assert.AreEqual("Alpha", sorted[1].Name);
        Assert.AreEqual("Widget", sorted[2].Name);
    }

    /// <summary>Verifies grouping orders groups by name and items by price.</summary>
    [TestMethod]
    public void GroupByNameThenPrice_OrdersGroupsAndItems()
    {
        var products = new List<Product>
        {
            new("Widget", null, 12.50m, 3),
            new("Widget", null, 10.00m, 1),
            new("Alpha", null, 9.99m, 2),
        };

        var groups = ProductSorter.GroupByNameThenPrice(products);

        Assert.AreEqual(2, groups.Count);
        Assert.AreEqual("Alpha", groups[0].Name);
        Assert.AreEqual(1, groups[0].Items.Count);
        Assert.AreEqual(10.00m, groups[1].Items[0].Price);
        Assert.AreEqual(12.50m, groups[1].Items[1].Price);
    }
}
