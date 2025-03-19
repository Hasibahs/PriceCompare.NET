using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main()
    {
        Console.Write("Enter a product name: ");
        string query = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(query))
        {
            Console.WriteLine("Invalid input. Please enter a valid product name.");
            return;
        }

        string apiUrl = $"http://localhost:3000/search?q={Uri.EscapeDataString(query)}";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();
                JArray products = JArray.Parse(responseData);

                if (products.Count == 0)
                {
                    Console.WriteLine("No products found.");
                }
                else
                {
                    Console.WriteLine("\nProducts:");
                    foreach (var product in products)
                    {
                        Console.WriteLine($"- {product["title"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }
        }

        // Keep the console open
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
