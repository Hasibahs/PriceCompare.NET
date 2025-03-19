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

        string apiUrl = $"http://localhost:3001/api/search?q={Uri.EscapeDataString(query)}";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(responseData);
                JArray products = (JArray)jsonResponse["products"];

                if (products == null || products.Count == 0)
                {
                    Console.WriteLine("No products found.");
                }
                else
                {
                    Console.WriteLine("\nProducts found:");
                    foreach (var product in products)
                    {
                        string title = product["title"]?.ToString() ?? "No title";

                        // Correctly extract the price information from priceV2.now
                        var price = product["priceV2"]?["now"]?.ToString() ?? "N/A";

                        string link = product["webPath"]?.ToString() ?? "#";

                        Console.WriteLine($"- {title}");
                        Console.WriteLine($"  Price: {price} EUR");
                        Console.WriteLine($"  Link: https://www.ah.nl{link}\n");
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
            }
        }

        // Keep console open
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
