using System;
using System.Data.SQLite;

class DatabaseCreator
{
    public static void CreateDatabase()
    {
        string dbPath = "Data Source=ProductData.db";  // Ensure this is correct

        Console.WriteLine($"Database path: {Environment.CurrentDirectory}\\ProductData.db"); // Debugging Output

        using (var connection = new SQLiteConnection(dbPath))
        {
            connection.Open();  // Critical step - creates the database if it doesn't exist

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Products (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ProductName TEXT NOT NULL,
                    Supermarket TEXT NOT NULL,
                    Price REAL NOT NULL,
                    Category TEXT NOT NULL,
                    DateUpdated TEXT NOT NULL
                );";

            using (var command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Database and table created successfully.");
        }
    }
}