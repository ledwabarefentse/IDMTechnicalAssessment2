using ProductSorting.Models;
using ProductSorting.Services;

Console.WriteLine("Product Sorting Console Application");
Console.WriteLine(new string('=', 36));
Console.WriteLine();
// Load once, then optionally apply normalization and sorting/grouping in the menu loop.
var products = LoadProducts("data/Technical Assessment Dev 1_ProductList.csv");
var normalizeNames = false;
while (true)
{
    Console.WriteLine("1. Sort by Price (ascending)");
    Console.WriteLine("2. Sort by Quantity (ascending)");
    Console.WriteLine("3. Sort by Product Name (ascending)");
    Console.WriteLine("4. Group by Product Name and sort by Price (ascending)");
    Console.WriteLine($"5. Toggle name normalization (currently {(normalizeNames ? "ON" : "OFF")})");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option (0-5): ");
    var choice = Console.ReadLine();
    if (choice == "0")
    {
        Console.WriteLine("Goodbye.");
        break;
    }
    if (choice == "5")
    {
        normalizeNames = !normalizeNames;
        Console.WriteLine($"Name normalization: {(normalizeNames ? "ON" : "OFF")}");
        continue;
    }

    // Normalization is opt-in to keep default behavior aligned with the raw CSV.
    var effectiveProducts = normalizeNames
        ? ProductNormalizer.NormalizeCategoryFromName(products)
        : products;

    if (choice == "4")
    {
        // Grouping prints a header per name with items sorted by price.
        var groups = ProductSorter.GroupByNameThenPrice(effectiveProducts);
        Console.WriteLine("Name | Category | Price | Quantity");
        foreach (var group in groups)
        {
            Console.WriteLine($"{group.Name}:");
            foreach (var product in group.Items)
            {
                Console.WriteLine($"  {product.Name} | {product.Category ?? "-"} | {product.Price:0.00} | {product.Quantity}");
            }
        }
        break;
    }

    products = choice switch
    {
        "1" => ProductSorter.SortByPriceAscending(effectiveProducts),
        "2" => ProductSorter.SortByQuantityAscending(effectiveProducts),
        "3" => ProductSorter.SortByNameAscending(effectiveProducts),
        _ => effectiveProducts.ToList()
    };
    Console.WriteLine("Name | Category | Price | Quantity");
    foreach (var product in products)
    {
        Console.WriteLine($"{product.Name} | {product.Category ?? "-"} | {product.Price:0.00} | {product.Quantity}");
    }
    break;
}

static List<Product> LoadProducts(string defaultPath)
{
    // Keep list selections stable across retries by reusing the last listing.
    var dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "data");
    var listed = new List<string>();
    while (true)
    {
        Console.Write($"Enter CSV file path (Enter for default: {defaultPath}), or type 'l' to list files: ");
        var input = Console.ReadLine();
        if (string.Equals(input?.Trim(), "l", StringComparison.OrdinalIgnoreCase))
        {
            listed = Directory.Exists(dataDirectory)
                ? Directory.GetFiles(dataDirectory, "*.csv")
                    .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                    .ToList()
                : new List<string>();

            for (var i = 0; i < listed.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(listed[i])}");
            }

            Console.Write("Select a number or paste a path (Enter for default): ");
            input = Console.ReadLine();
        }
        if (int.TryParse(input, out var selection) && selection >= 1 && selection <= listed.Count)
        {
            input = listed[selection - 1];
        }

        var path = string.IsNullOrWhiteSpace(input) ? defaultPath : input.Trim().Trim('\"');
        try
        {
            return CsvProductReader.ReadProducts(path);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or FormatException)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
