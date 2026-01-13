using ProductSorting.Models;

namespace ProductSorting.Services;

/// <summary>Provides sorting and grouping operations for products.</summary>
public static class ProductSorter
{
    /// <summary>Sorts products by price ascending, then by name.</summary>
    /// <param name="products">Products to sort.</param>
    /// <returns>Sorted list by price then name.</returns>
    public static List<Product> SortByPriceAscending(IEnumerable<Product> products)
    {
        // Sort by price, then by name for stable and predictable output.
        return products
            .OrderBy(p => p.Price)
            .ThenBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>Sorts products by quantity ascending, then by name.</summary>
    /// <param name="products">Products to sort.</param>
    /// <returns>Sorted list by quantity then name.</returns>
    public static List<Product> SortByQuantityAscending(IEnumerable<Product> products)
    {
        // Sort by quantity, then by name for stable and predictable output.
        return products
            .OrderBy(p => p.Quantity)
            .ThenBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>Sorts products by name ascending, then by price.</summary>
    /// <param name="products">Products to sort.</param>
    /// <returns>Sorted list by name then price.</returns>
    public static List<Product> SortByNameAscending(IEnumerable<Product> products)
    {
        // Sort by name and then price to keep items with the same name ordered.
        return products
            .OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            .ThenBy(p => p.Price)
            .ToList();
    }

    /// <summary>Groups products by name and sorts each group by price ascending.</summary>
    /// <param name="products">Products to group.</param>
    /// <returns>Grouped products ordered by name with items sorted by price.</returns>
    public static List<ProductGroup> GroupByNameThenPrice(IEnumerable<Product> products)
    {
        // Group by name and then sort within each group by price.
        return products
            .GroupBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase)
            .Select(group => new ProductGroup(
                group.Key,
                // Preserve original name within the group while sorting by price.
                group.OrderBy(p => p.Price).ToList()))
            .ToList();
    }
}
