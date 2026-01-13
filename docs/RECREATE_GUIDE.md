# Recreate Guide: Product Sorting Console Application

This guide is a step-by-step, from-zero walkthrough to rebuild the project and explain the reasoning behind each step. It includes both high-level and low-level instructions and ties decisions to SDLC, Agile, and OOP principles. The walkthrough is intentionally incremental: you deliver small working slices, test them, then add complexity.

## 1) High-Level Roadmap (What You Will Build)
- A C# console app that reads a CSV file of products.
- Menu options to sort by price, quantity, or name, and to group by name then price.
- Robust error handling for missing files and malformed CSVs.
- Optional name normalization to split trailing category tokens (e.g., "Widget AA").
- Unit tests for parsing, sorting, and normalization.
- Documentation that explains usage and design choices.

## 2) SDLC and Agile Framing (How You Approach It)
- Requirements: read the brief and list explicit acceptance criteria.
- Design: pick a simple architecture (models + services + console UI).
- Implementation: build the core path first (read CSV -> sort -> display), then expand.
- Verification: add unit tests and run them.
- Release: document usage and package or publish the solution.

Agile approach:
- Start with the smallest working version (MVP): read CSV and print sorted output.
- Iterate: add menu options, grouping, error handling, tests, and optional normalization.
- Use short feedback loops: run the app and tests after each increment.

## 3) Project Setup (Zero to Solution)

### 3.1 Create the folder structure
```
IDMTechnicalAssessment/
  src/
    ProductSorting/
      Models/
      Services/
  tests/
    ProductSorting.Tests/
```

### 3.2 Create the solution and projects
If using the CLI:
```
dotnet new sln -n IDMTechnicalAssessment

dotnet new console -n ProductSorting -o src/ProductSorting

dotnet new mstest -n ProductSorting.Tests -o tests/ProductSorting.Tests

dotnet sln IDMTechnicalAssessment.sln add src/ProductSorting/ProductSorting.csproj

dotnet sln IDMTechnicalAssessment.sln add tests/ProductSorting.Tests/ProductSorting.Tests.csproj

dotnet add tests/ProductSorting.Tests/ProductSorting.Tests.csproj reference src/ProductSorting/ProductSorting.csproj
```

### 3.3 Verify the project runs
```
dotnet run --project src/ProductSorting
```

## 4) Low-Level Build Steps (Feature by Feature)

### Step 1: Define the data model (OOP foundation)
Create records to represent products and grouped results.
- `Product` holds Name, optional Category, Price, Quantity.
- `ProductGroup` holds group name and items list.

Files:
- `src/ProductSorting/Models/Product.cs`
- `src/ProductSorting/Models/ProductGroup.cs`

Principles:
- Encapsulation: keep model responsibilities minimal and clear.
- Immutability: use records for simpler reasoning and testing.

### Step 2: Implement CSV parsing (core I/O)
Create a reader that:
- Accepts 3-column and 4-column formats.
- Supports quoted fields and commas inside quotes.
- Detects an optional header line.
- Throws clear FormatException messages when input is invalid.

File:
- `src/ProductSorting/Services/CsvProductReader.cs`

Why:
- Separation of concerns keeps parsing logic independent from UI.

### Step 3: Implement sorting/grouping (business logic)
Create a dedicated sorter service with methods:
- SortByPriceAscending
- SortByQuantityAscending
- SortByNameAscending
- GroupByNameThenPrice

File:
- `src/ProductSorting/Services/ProductSorter.cs`

Why:
- Pure functions are easy to test and reason about.

### Step 4: Build the console UI (interaction layer)
Add:
- Prompt for CSV file path.
- Optional listing of CSV files in the current folder.
- Menu with sorting and grouping options.
- Output formatting as a table (columns, padding, headers).
- Friendly error messages and retries.

File:
- `src/ProductSorting/Program.cs`

### Step 5: Add optional name normalization (data quality handling)
For the dataset with embedded category in the product name:
- If the Category column is missing, optionally split trailing 1-2 letter tokens.
- Example: "Widget AA" -> Name = "Widget", Category = "AA".
- Make this opt-in via menu toggle (so default behavior is spec-compliant).

File:
- `src/ProductSorting/Services/ProductNormalizer.cs`

### Step 6: Add unit tests (verification)
Write tests for:
- CSV parsing (valid rows, quoted fields, invalid rows).
- Sorting logic.
- Name normalization (1-2 letter suffixes).

Files:
- `tests/ProductSorting.Tests/CsvProductReaderTests.cs`
- `tests/ProductSorting.Tests/ProductSorterTests.cs`
- `tests/ProductSorting.Tests/ProductNormalizerTests.cs`

### Step 7: Document the project (handoff readiness)
Add clear documentation describing:
- What the app does.
- CSV formats supported.
- How to run the app and tests.
- The optional normalization feature and why it exists.

File:
- `README.md`

## 5) Run and Validate
```
dotnet run --project src/ProductSorting

dotnet test
```

Manual checks:
- Load both CSV test files.
- Validate all menu options.
- Toggle name normalization and confirm grouping behavior.

## 6) Agile Delivery Notes (How to Present It)
Suggested demo flow:
1) Show MVP (read CSV and list products).
2) Add sorting menu and show outputs.
3) Add grouping option.
4) Demonstrate error handling with a bad path or malformed file.
5) Run tests.
6) Explain optional normalization as an explicit tradeoff.

## 7) OOP and Design Principles Applied
- Single Responsibility: parsing, sorting, normalization, UI are separate.
- Open/Closed: new sort options can be added without changing core models.
- Testability: logic classes are pure and unit-test-friendly.
- Readability: consistent naming and clear method roles.

## 8) Hand-off Checklist

## 9) Detailed Implementation Walkthrough (High + Low Level)

This section is intentionally verbose and incremental. You should not finish the entire solution in one go. Each iteration delivers a small working slice and ends with a verification step before moving on.

### 9.1 Initialize the repository (Iteration 0: Skeleton)
1) Create the root folder.
```
mkdir IDMTechnicalAssessment
```
2) Enter the folder.
```
cd IDMTechnicalAssessment
```
3) Create the solution file.
```
dotnet new sln -n IDMTechnicalAssessment
```
4) Create the console project.
```
dotnet new console -n ProductSorting -o src/ProductSorting
```
5) Create the test project.
```
dotnet new mstest -n ProductSorting.Tests -o tests/ProductSorting.Tests
```
6) Add projects to the solution.
```
dotnet sln IDMTechnicalAssessment.sln add src/ProductSorting/ProductSorting.csproj
dotnet sln IDMTechnicalAssessment.sln add tests/ProductSorting.Tests/ProductSorting.Tests.csproj
```
7) Reference the app project from the test project.
```
dotnet add tests/ProductSorting.Tests/ProductSorting.Tests.csproj reference src/ProductSorting/ProductSorting.csproj
```
8) Verify the empty app runs.
```
dotnet run --project src/ProductSorting
```
What’s next: add the smallest possible feature in Iteration 1.

### 9.2 Iteration 1: MVP (read CSV and display raw rows)
Goal: prove the app can read a file and print something without any sorting yet.

1) Create the models folder.
```
mkdir -p src/ProductSorting/Models
```
2) Create `Product.cs` with a `Product` record.
- Include `Name`, optional `Category`, `Price`, `Quantity`.
3) Create the services folder.
```
mkdir -p src/ProductSorting/Services
```
4) Create `CsvProductReader.cs` with a minimal `ReadProducts` method.
- Validate file exists.
- Read lines.
- Parse 3 columns only.
- Return a list of `Product`.
5) Update `Program.cs` to:
- Load the default CSV.
- Print each product line (name, price, qty).
6) Run the app to validate the MVP.
```
dotnet run --project src/ProductSorting
```
What’s next: improve CSV parsing and add proper error handling in Iteration 2.

### 9.3 Iteration 2: Robust CSV parsing and error handling
Goal: support headers, quoted fields, and errors without crashing.

1) Expand `CsvProductReader` to:
- Detect and skip an optional header.
- Support quoted fields and commas inside quotes.
- Throw `FormatException` with line numbers.
2) Update `Program.cs` to:
- Catch parsing errors and allow retry.
- Prompt for file paths with a default.
3) Run with both CSV files to confirm.
```
dotnet run --project src/ProductSorting
```
What’s next: add sorting options in Iteration 3.

### 9.4 Iteration 3: Sorting functionality
Goal: expose sorting options and verify output changes.

1) Create `ProductSorter.cs` in `Services`.
2) Implement:
- `SortByPriceAscending`
- `SortByQuantityAscending`
- `SortByNameAscending`
3) Update `Program.cs` to add menu options 1–3.
4) Run and manually check each sort.
```
dotnet run --project src/ProductSorting
```
What’s next: add grouping in Iteration 4.

### 9.5 Iteration 4: Grouping by name
Goal: group by product name and sort within groups by price.

1) Add `ProductGroup.cs`.
2) Add `GroupByNameThenPrice` in `ProductSorter`.
3) Update `Program.cs` with menu option 4 and grouped output.
4) Run and confirm group display.
```
dotnet run --project src/ProductSorting
```
What’s next: add file listing and UI improvements in Iteration 5.

### 9.6 Iteration 5: File selection UX
Goal: reduce friction when the user does not know the full path.

1) Add `ListCsvFiles` and a `l` option to list files.
2) Allow a full path to be pasted after listing.
3) Run and verify file selection.
```
dotnet run --project src/ProductSorting
```
What’s next: add optional name normalization in Iteration 6.

### 9.7 Iteration 6: Optional name normalization
Goal: handle the dataset that embeds category in the name without changing default behavior.

1) Create `ProductNormalizer.cs`.
2) Implement `NormalizeCategoryFromName`:
- Split a trailing 1–2 letter token into Category.
3) Add a menu toggle (option 6) and apply normalization to display/sort/group when enabled.
4) Run and compare grouping with normalization on/off.
```
dotnet run --project src/ProductSorting
```
What’s next: add automated tests in Iteration 7.

### 9.8 Iteration 7: Unit tests
Goal: lock in behavior and prevent regressions.

1) Add tests for CSV parsing.
2) Add tests for sorting/grouping.
3) Add tests for normalization.
4) Run tests.
```
dotnet test
```
What’s next: finalize documentation in Iteration 8.

### 9.9 Iteration 8: Documentation and final validation
Goal: make the project easy to hand off and explain.

1) Update `README.md` with:
- How to run and test.
- CSV formats supported.
- Normalization toggle behavior.
2) Update this guide if anything changed.
3) Run the app with both CSV files and verify the menu.
4) Run tests again.
```
dotnet run --project src/ProductSorting
dotnet test
```
5) Confirm the hand-off checklist is satisfied.
