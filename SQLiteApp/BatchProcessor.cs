using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading;
using System.Threading.Tasks;

class BatchProcessor
{
    private const int BatchSize = 500;

    public async Task ProcessDataInBatchesAsync(List<string> productNames)
    {
        for (int i = 0; i < productNames.Count; i += BatchSize)
        {
            var batch = productNames.GetRange(i, Math.Min(BatchSize, productNames.Count - i));

            var tasks = new List<Task>();

            foreach (var product in batch)
            {
                tasks.Add(Task.Run(async () =>
                {
                    await ProcessProductAsync(product);
                }));
            }

            await Task.WhenAll(tasks);  
            Console.WriteLine($"Batch processed: {i / BatchSize + 1}");
        }

        Console.WriteLine("All batches processed successfully.");
    }

    public async Task ProcessProductAsync(string productName)
    {
        string dbPath = "Data Source=ProductData.db";

        using (var connection = new SQLiteConnection(dbPath))
        {
            await connection.OpenAsync();

            string query = "SELECT * FROM Products WHERE ProductName = @ProductName";

            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductName", productName);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        Console.WriteLine($"{reader["ProductName"]} - {reader["Supermarket"]} - €{reader["Price"]}");
                    }
                    else
                    {
                        Console.WriteLine($"Product not found: {productName}");
                    }
                }
            }
        }
    }
}
