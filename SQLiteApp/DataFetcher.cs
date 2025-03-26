using System;
using System.Data.SQLite;
using System.Threading.Tasks;

class DataFetcher
{
    public async Task FetchDataAsync(string productName)
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
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"{reader["ProductName"]} - {reader["Supermarket"]} - €{reader["Price"]}");
                    }
                }
            }
        }
    }
}
