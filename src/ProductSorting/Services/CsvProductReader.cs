using System.Globalization;
using System.Text;
using ProductSorting.Models;

namespace ProductSorting.Services;

public static class CsvProductReader
{
    /// <summary>Reads products from a CSV file and validates basic format.</summary>
    /// <param name="filePath">Path to the CSV file.</param>
    /// <returns>List of parsed products.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
    /// <exception cref="FormatException">Thrown when a row is malformed.</exception>
    public static List<Product> ReadProducts(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("CSV file not found.", filePath);
        }

        var lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
        {
            return new List<Product>();
        }

        // Skip a header row if the first line looks like column names.
        var startIndex = LooksLikeHeader(lines[0]) ? 1 : 0;
        var products = new List<Product>();

        for (var i = startIndex; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var fields = ParseLine(line);
            if (fields.Count != 3 && fields.Count != 4)
            {
                throw new FormatException($"Invalid CSV format on line {i + 1}.");
            }

            var name = fields[0].Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new FormatException($"Missing product name on line {i + 1}.");
            }
            var category = fields.Count == 4 ? fields[1].Trim() : null;
            if (string.IsNullOrWhiteSpace(category))
            {
                category = null;
            }

            var priceField = fields.Count == 4 ? fields[2] : fields[1];
            var quantityField = fields.Count == 4 ? fields[3] : fields[2];

            if (!decimal.TryParse(priceField, NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
            {
                throw new FormatException($"Invalid price on line {i + 1}.");
            }

            if (!int.TryParse(quantityField, NumberStyles.Integer, CultureInfo.InvariantCulture, out var quantity))
            {
                throw new FormatException($"Invalid quantity on line {i + 1}.");
            }

            products.Add(new Product(name, category, price, quantity));
        }

        return products;
    }

    /// <summary>Detects whether a line looks like a header row.</summary>
    /// <param name="line">CSV line to inspect.</param>
    /// <returns>True if the line appears to be a header.</returns>
    private static bool LooksLikeHeader(string line)
    {
        var fields = ParseLine(line);
        if (fields.Count < 3)
        {
            return false;
        }

        var priceIndex = fields.Count >= 4 ? 2 : 1;
        var quantityIndex = fields.Count >= 4 ? 3 : 2;

        return fields[0].Contains("Product", StringComparison.OrdinalIgnoreCase)
            && fields[priceIndex].Contains("Price", StringComparison.OrdinalIgnoreCase)
            && fields[quantityIndex].Contains("Quantity", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Splits a CSV line into fields, honoring quoted values.</summary>
    /// <param name="line">CSV line to parse.</param>
    /// <returns>Parsed fields in order.</returns>
    private static List<string> ParseLine(string line)
    {
        var fields = new List<string>();
        var current = new StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < line.Length; i++)
        {
            var ch = line[i];
            if (ch == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }

                continue;
            }

            if (ch == ',' && !inQuotes)
            {
                fields.Add(current.ToString());
                current.Clear();
                continue;
            }

            current.Append(ch);
        }

        fields.Add(current.ToString());
        return fields;
    }
}
