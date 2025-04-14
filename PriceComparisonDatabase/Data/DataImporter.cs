using System.Text.Json;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;

namespace PriceComparisonApp
{
    public static class DataImporter
    {

        public static async Task ImportDataAsync(IEnumerable<string> jsonFilePaths, string dbPath)
        {
            var allProducts = new ConcurrentBag<Product>();

            // parse the JSON in parallel with Task.Run
            var tasks = jsonFilePaths.Select(path => Task.Run(async () =>
            {
                try
                {
                    // Read the entire file text
                    string json = await File.ReadAllTextAsync(path).ConfigureAwait(false);

                    // Parse into a List<Root>
                    var supermarkets = JsonSerializer.Deserialize<List<Root>>(json);

                    // If it parsed successfully, flatten each top-level item
                    if (supermarkets != null)
                    {
                        foreach (var sup in supermarkets)
                        {
                            // sup.SupermarketName = "ah", "jumbo", etc.
                            // sup.Products is the array of products

                            foreach (var item in sup.Products)
                            {
                                var p = new Product
                                {
                                    // Copy data from the JSON model to the DB model
                                    Name = item.ProductName,
                                    Link = item.Link,
                                    Price = item.Price,
                                    Size = item.Size,
                                    Supermarket = sup.SupermarketName
                                };

                                // Add to thread-safe collection
                                allProducts.Add(p);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading '{path}': {ex.Message}");
                }
            }));

            // Wait until all parallel tasks finish
            await Task.WhenAll(tasks).ConfigureAwait(false);

            // 2) Insert them into SQLite via EF
            using var db = new ProductContext(dbPath);

            // Create the DB if not exists
            await db.Database.EnsureCreatedAsync().ConfigureAwait(false);

            // Wrap inserts in a transaction for efficiency
            using var transaction = await db.Database.BeginTransactionAsync().ConfigureAwait(false);

            try
            {
                // Bulk-insert with EF
                await db.Products.AddRangeAsync(allProducts).ConfigureAwait(false);
                await db.SaveChangesAsync().ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DB insert error: {ex.Message}");
                await transaction.RollbackAsync().ConfigureAwait(false);
            }
        }
    }
}
