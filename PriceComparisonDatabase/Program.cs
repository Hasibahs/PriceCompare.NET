using PriceComparisonApp;

namespace PriceComparisonApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            // reference it by filename:
            var filePaths = new List<string>
            {
                "supermarkets (1).json"
            };


            string dbPath = Path.Combine(AppContext.BaseDirectory, "products.db");

            Console.WriteLine("Starting import process...");

            await DataImporter.ImportDataAsync(filePaths, dbPath);

            Console.WriteLine("Import completed. Press ENTER to show database contents...");
            Console.ReadLine();

            //  Show how many products were inserted
            using (var db = new ProductContext(dbPath))
            {
                var totalProducts = db.Products.Count();
                Console.WriteLine($"Total products in DB: {totalProducts}");

                var firstTen = db.Products.Take(10).ToList();
                foreach (var p in firstTen)
                {
                    Console.WriteLine($"ID={p.ProductId}, Name={p.Name}, Price={p.Price}, Size={p.Size}, Shop={p.Supermarket}");
                }
            }

            Console.WriteLine("Done! Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
