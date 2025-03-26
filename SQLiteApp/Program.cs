using System;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Database Creation...");

        // Create Database
        DatabaseCreator.CreateDatabase();

        // Insert Data
        DataInserter.InsertData();

        // Process Data in Batches
        var productNames = new List<string> 
        {
            "Milk", "Bread", "Butter", "Eggs", "Cheese", "Tomatoes", "Chicken", "Rice"
        };

        var processor = new BatchProcessor();
        await processor.ProcessDataInBatchesAsync(productNames);
    }
}
