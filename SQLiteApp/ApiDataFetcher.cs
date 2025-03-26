using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Data.SQLite;

class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}

class ApiDataFetcher
{
    private static readonly HttpClient client = new HttpClient();

    public async Task<List<Product>> FetchDataFromApiAsync()
    {
        try
        {
            string apiUrl = "https://fakestoreapi.com/products";  
            var response = await client.GetStringAsync(apiUrl);

            var products = JsonSerializer.Deserialize<List<Product>>(response);
            return products ?? new List<Product>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching data: {ex.Message}");
            return new List<Product>();
        }
    }
}
