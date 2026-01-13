# Product Sorting Console Application

A C# console app that reads a CSV file of products, offers sorting/grouping options, and prints results to the console. No external NuGet packages are used.

## Requirements
- .NET SDK

## Project Layout
```
src/ProductSorting/        Console application
tests/ProductSorting.Tests Unit tests
data/                      Sample CSV files
docs/                      Assessment brief
```

## CSV Formats Supported
The app accepts two CSV formats:

3-column format:
```
Product Name,Price (ZAR),Quantity
Widget A,10.99,100
```

4-column format:
```
Product Name,Category,Price (ZAR),Quantity
Widget,A,10.99,100
```

## Run the App
```
dotnet run --project src/ProductSorting
```

If you want to use the included sample data, keep the CSVs in `data/`.

When prompted:
- Press Enter to use the default CSV.
- Type `l` to list CSVs from the `data` folder.
- After listing, enter a number or paste a full path.

## Menu Options
1. Sort by Price (ascending)
2. Sort by Quantity (ascending)
3. Sort by Product Name (ascending)
4. Group by Product Name and sort by Price (ascending)
5. Toggle name normalization (ON/OFF)
0. Exit

Output columns (header is printed before results):
```
Name | Category | Price | Quantity
```

## Example Session
```
Enter CSV file path (Enter for default: data/Technical Assessment Dev 1_ProductList.csv), or type 'l' to list files: l
1. Technical Assessment Dev 1_ProductList.csv
2. Technical Assessment Dev 1_ProductList_2.csv
Select a number or paste a path (Enter for default): 2
1. Sort by Price (ascending)
2. Sort by Quantity (ascending)
3. Sort by Product Name (ascending)
4. Group by Product Name and sort by Price (ascending)
5. Toggle name normalization (currently OFF)
Choose an option (1-5): 4
```

## Name Normalization (Optional)
If a product has no Category, you can toggle normalization to split a trailing 1–2 letter token from the name into the Category.
Example:
```
Widget AA -> Name: Widget, Category: AA
```
This is opt-in and does not change the default behavior.

## Run Tests
```
dotnet test
```

## Error Handling
- Missing files raise a clear message and re-prompt.
- Invalid CSV format surfaces line-level errors.

## Notes
- CSV parsing supports quoted fields and commas inside quotes.
- Errors (missing file, invalid format) are handled with friendly prompts.
