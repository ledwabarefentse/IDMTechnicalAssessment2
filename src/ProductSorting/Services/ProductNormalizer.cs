using ProductSorting.Models;

namespace ProductSorting.Services;

/// <summary>Applies optional heuristics to normalize product names and categories.</summary>
public static class ProductNormalizer
{
    /// <summary>Normalizes products by extracting a trailing category token from the name when missing.</summary>
    /// <param name="products">Products to normalize.</param>
    /// <returns>List of products with inferred categories when possible.</returns>
    public static List<Product> NormalizeCategoryFromName(IEnumerable<Product> products)
    {
        var normalized = new List<Product>();
        foreach (var product in products)
        {
            if (!string.IsNullOrWhiteSpace(product.Category))
            {
                normalized.Add(product);
                continue;
            }

            // Heuristic: split a trailing single-letter token (e.g., "Widget A") into name + category.
            if (TrySplitName(product.Name, out var baseName, out var category))
            {
                normalized.Add(product with { Name = baseName, Category = category });
            }
            else
            {
                normalized.Add(product);
            }
        }

        return normalized;
    }

    /// <summary>Splits a trailing 1-2 letter token from a name into a category.</summary>
    /// <param name="name">Name to inspect.</param>
    /// <param name="baseName">Name without the category token.</param>
    /// <param name="category">Extracted category token.</param>
    /// <returns>True when a trailing category token was found.</returns>
    private static bool TrySplitName(string name, out string baseName, out string category)
    {
        baseName = name;
        category = string.Empty;

        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            return false;
        }

        // Only treat short trailing tokens as categories (e.g., "AA").
        var last = parts[^1];
        if (last.Length is >= 1 and <= 2 && last.All(char.IsLetter))
        {
            category = last.ToUpperInvariant();
            baseName = string.Join(' ', parts.Take(parts.Length - 1));
            return !string.IsNullOrWhiteSpace(baseName);
        }

        return false;
    }
}
