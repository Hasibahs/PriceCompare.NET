using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PriceComparisonApp.Models;

namespace PriceComparisonApp.Services
{
    public static class BackendService
    {
        public static async Task<List<ProductResult>> SearchProductsAsync(string productName, string storeFilter = null)
        {
            // Simulate an async call (e.g., calling an API or database)
            await Task.Delay(1000);

            // Dummy data
            var allResults = new List<ProductResult>
            {
                new ProductResult { Store = "Albert Heijn", Price = 1.10 },
                new ProductResult { Store = "Jumbo", Price = 1.05 },
                new ProductResult { Store = "Lidl", Price = 1.20 },
                new ProductResult { Store = "Aldi", Price = 0.99 }
            };

            // Filter by store name if selected
            if (!string.IsNullOrWhiteSpace(storeFilter))
            {
                allResults = allResults
                    .Where(r => r.Store.Contains(storeFilter))
                    .ToList();
            }

            // Mark the cheapest as "IsBestDeal"
            if (allResults.Count > 0)
            {
                double minPrice = allResults.Min(r => r.Price);
                allResults.ForEach(r => r.IsBestDeal = (r.Price == minPrice));
            }

            // Return the (possibly filtered) results
            return allResults;
        }
    }
}
