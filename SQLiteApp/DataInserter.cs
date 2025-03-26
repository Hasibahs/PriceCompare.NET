using System;
using System.Data.SQLite;

class DataInserter
{
    public static void InsertData()
    {
        string dbPath = "Data Source=ProductData.db";
        var random = new Random();

        string[] supermarkets = { "Albert Heijn", "Jumbo", "Lidl", "Aldi", "Coop" };
        string[] products = { "Milk", "Bread", "Butter", "Eggs", "Cheese", "Tomatoes", "Chicken", "Rice" };

        using (var connection = new SQLiteConnection(dbPath))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                for (int i = 0; i < 100000; i++)
                {
                    string product = products[random.Next(products.Length)];
                    string supermarket = supermarkets[random.Next(supermarkets.Length)];
                    double price = Math.Round(random.NextDouble() * 20, 2);
                    string date = DateTime.Now.AddDays(-random.Next(365)).ToShortDateString();

                    string insertQuery = @"
                        INSERT INTO Products (ProductName, Supermarket, Price, Category, DateUpdated)
                        VALUES (@ProductName, @Supermarket, @Price, @Category, @DateUpdated);";

                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", product);
                        command.Parameters.AddWithValue("@Supermarket", supermarket);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Category", "Food");
                        command.Parameters.AddWithValue("@DateUpdated", date);

                        command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
            }

            Console.WriteLine("100,000 records inserted successfully.");
        }
    }
}
