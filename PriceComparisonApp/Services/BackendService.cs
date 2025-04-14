using System.Net.Http.Json;
using PriceComparisonApp.Models;

namespace PriceComparisonApp.Services
{
    public static class BackendService
    {
        private static readonly HttpClient httpClient = new HttpClient();
        // Base URL for your ASP.NET Core API server – adjust if needed.
        private static readonly string baseUrl = "http://localhost:5094/api/products/";

        public static async Task<List<ProductResult>> SearchProductsAsync(string productName, string storeFilter = null)
        {
            var url = $"{baseUrl}search?q={Uri.EscapeDataString(productName)}";
            try
            {
                // Get products from your backend search endpoint.
                var products = await httpClient.GetFromJsonAsync<List<Product>>(url);
                if (products == null)
                    return new List<ProductResult>();

                // Map the backend Product model to the front-end ProductResult model.
                var results = products.Select(p => new ProductResult
                {
                    Store = p.Supermarket,
                    Price = (double)p.Price,
                    ProductName = p.Name,
                    ProductDetails = p.Size,
                    Link = p.Link,
                    IsBestDeal = false
                }).ToList();

                // Optionally filter by store name.
                if (!string.IsNullOrWhiteSpace(storeFilter))
                {
                    results = results
                        .Where(r => r.Store.Contains(storeFilter, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Mark the best deal (lowest price).
                if (results.Any())
                {
                    var minPrice = results.Min(r => r.Price);
                    results.ForEach(r => r.IsBestDeal = r.Price == minPrice);
                }

                return results;
            }
            catch (Exception ex)
            {
                // Log or handle exceptions as needed.
                return new List<ProductResult>();
            }
        }
    }

    // Product model for deserialization (matching the backend API)
    public class Product
    {
        public int ProductId { get; set; }
        public string Supermarket { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
