using PriceComparisonApp.Models;

namespace PriceComparisonApp.Services
{
    public static class BackendService
    {
        public static async Task<List<ProductResult>> SearchProductsAsync(string productName, string storeFilter = null)
        {
            await Task.Delay(500); // Simulate network call

            var allResults = new List<ProductResult>
            {
                new ProductResult { Store = "Albert Heijn", Price = 1.10 },
                new ProductResult { Store = "Jumbo", Price = 1.05 },
                new ProductResult { Store = "Lidl", Price = 1.20 },
                new ProductResult { Store = "Aldi", Price = 0.99 }
            };

            if (!string.IsNullOrWhiteSpace(storeFilter))
            {
                allResults = allResults.Where(r => r.Store.Contains(storeFilter)).ToList();
            }

            var minPrice = allResults.Min(r => r.Price);
            allResults.ForEach(r => r.IsBestDeal = r.Price == minPrice);

            return allResults;
        }
    }
}
