namespace ProductSorting.Models;

/// <summary>Represents a named group of products.</summary>
/// <param name="Name">Group name.</param>
/// <param name="Items">Products in the group.</param>
public sealed record ProductGroup(string Name, List<Product> Items);
