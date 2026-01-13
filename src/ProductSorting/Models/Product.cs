namespace ProductSorting.Models;

/// <summary>Represents a product record from the CSV.</summary>
/// <param name="Name">Product name.</param>
/// <param name="Category">Optional product category.</param>
/// <param name="Price">Price in ZAR.</param>
/// <param name="Quantity">Available quantity.</param>
public sealed record Product(string Name, string? Category, decimal Price, int Quantity);
