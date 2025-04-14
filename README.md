# PriceCompare.NET

This is a C# threading-based Price Comparison App that allows users to search for a product and compare prices from various supermarkets (e.g., Albert Heijn, Jumbo, Aldi, etc.). The application consists of two main parts:

- `PriceComparisonAPI` — a backend ASP.NET Core Web API that aggregates product prices.
- `PriceComparisonApp` — a .NET MAUI frontend that displays the price comparison UI.

---

## Requirements

- Visual Studio 2022 or newer (with .NET MAUI and ASP.NET Core workloads installed)
- .NET 8 SDK

---

## How to Run the Project

> Make sure to **run the API first** before launching the app.

### Step 1: Run the API

1. Open **File Explorer** and navigate to the `PriceComparisonAPI` folder.
2. Double-click the file: `PriceComparisonAPI2.sln`
3. This will open the API project in **Visual Studio**.
4. Click the green **Run** (▶️) button at the top.
5. After the API launches, copy the URL shown in the **Output** or **Terminal** window (usually something like `https://localhost:5001`).

### Step 2: Run the MAUI App

1. Go back to the **main project directory** (where the zip was extracted or the repo was cloned).
2. Double-click the `PriceComparisonApp.sln` file to open it in Visual Studio.
3. Click the **Run** (▶️) button at the top.
4. The MAUI app will launch. In the search bar, enter a product name (e.g., `milk`, `apple`, `cola`) and the app will display price comparisons across multiple supermarkets.

---

## C# Threading

This project demonstrates usage of **C# threading techniques** including:

- `Task.Run()` for async CPU-bound operations
- Thread pooling for efficient background processing
- PLINQ for parallel product data processing

These techniques ensure that both the API and UI remain responsive while fetching and comparing data.
