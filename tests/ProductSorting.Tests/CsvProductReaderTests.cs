using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductSorting.Services;

namespace ProductSorting.Tests;

[TestClass]
public class CsvProductReaderTests
{
    [TestMethod]
    public void ReadProducts_ParsesThreeColumnCsvWithHeader()
    {
        var path = WriteTempCsv("""
Product Name,Price (ZAR),Quantity
Widget A,10.99,100
Gadget B,24.95,50
""");

        try
        {
            var products = CsvProductReader.ReadProducts(path);

            Assert.AreEqual(2, products.Count);
            Assert.AreEqual("Widget A", products[0].Name);
            Assert.IsNull(products[0].Category);
            Assert.AreEqual(10.99m, products[0].Price);
            Assert.AreEqual(100, products[0].Quantity);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void ReadProducts_ParsesFourColumnCsvWithHeader()
    {
        var path = WriteTempCsv("""
Product Name,Category,Price (ZAR),Quantity
Widget,A,10.99,100
""");

        try
        {
            var products = CsvProductReader.ReadProducts(path);

            Assert.AreEqual(1, products.Count);
            Assert.AreEqual("Widget", products[0].Name);
            Assert.AreEqual("A", products[0].Category);
            Assert.AreEqual(10.99m, products[0].Price);
            Assert.AreEqual(100, products[0].Quantity);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void ReadProducts_ParsesQuotedFieldsWithCommas()
    {
        var path = WriteTempCsv("""
Product Name,Price (ZAR),Quantity
"Widget, Large",10.00,5
""");

        try
        {
            var products = CsvProductReader.ReadProducts(path);

            Assert.AreEqual(1, products.Count);
            Assert.AreEqual("Widget, Large", products[0].Name);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void ReadProducts_ThrowsForMissingFile()
    {
        Assert.ThrowsException<FileNotFoundException>(() =>
            CsvProductReader.ReadProducts("missing-file.csv"));
    }

    [TestMethod]
    public void ReadProducts_ThrowsForInvalidColumnCount()
    {
        var path = WriteTempCsv("""
Product Name,Price (ZAR),Quantity
Widget A,10.99
""");

        try
        {
            Assert.ThrowsException<FormatException>(() =>
                CsvProductReader.ReadProducts(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void ReadProducts_ThrowsForMissingName()
    {
        var path = WriteTempCsv("""
Product Name,Price (ZAR),Quantity
,10.99,1
""");

        try
        {
            Assert.ThrowsException<FormatException>(() =>
                CsvProductReader.ReadProducts(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void ReadProducts_ThrowsForInvalidPrice()
    {
        var path = WriteTempCsv("""
Product Name,Price (ZAR),Quantity
Widget A,not-a-number,1
""");

        try
        {
            Assert.ThrowsException<FormatException>(() =>
                CsvProductReader.ReadProducts(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void ReadProducts_ThrowsForInvalidQuantity()
    {
        var path = WriteTempCsv("""
Product Name,Price (ZAR),Quantity
Widget A,10.99,not-a-number
""");

        try
        {
            Assert.ThrowsException<FormatException>(() =>
                CsvProductReader.ReadProducts(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public void ReadProducts_ParsesWithoutHeader()
    {
        var path = WriteTempCsv("""
Widget A,10.99,100
Gadget B,24.95,50
""");

        try
        {
            var products = CsvProductReader.ReadProducts(path);

            Assert.AreEqual(2, products.Count);
            Assert.AreEqual("Widget A", products[0].Name);
            Assert.AreEqual("Gadget B", products[1].Name);
        }
        finally
        {
            File.Delete(path);
        }
    }

    private static string WriteTempCsv(string content)
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, content.Replace("\r\n", "\n"));
        return path;
    }
}
