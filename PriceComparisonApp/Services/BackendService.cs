// PriceComparisonApp/Services/BackendService.cs
using System.Net.Http.Json;
using PriceComparisonApp.Models;

namespace PriceComparisonApp.Services
{
    public static class BackendService
    {
        private static readonly HttpClient httpClient = new HttpClient();
        // Base URL for your Express API server – adjust if needed.
        private static readonly string baseUrl = "http://localhost:3001/api";

        public static async Task<List<ProductResult>> SearchProductsAsync(string productName, string storeFilter = null)
        {
            // Call each API asynchronously
            var jumboTask = GetJumboProducts(productName);
            var aldiTask = GetAldiProducts(productName);
            var ahTask = GetAHProducts(productName);

            await Task.WhenAll(jumboTask, aldiTask, ahTask);

            // Combine results from all stores
            var allResults = jumboTask.Result
                .Concat(aldiTask.Result)
                .Concat(ahTask.Result)
                .ToList();

            // Optionally filter by store name
            if (!string.IsNullOrWhiteSpace(storeFilter))
            {
                allResults = allResults
                    .Where(r => r.Store.Contains(storeFilter, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Sort the results by price (lowest to highest)
            allResults = allResults.OrderBy(r => r.Price).ToList();

            // Mark the best deal (lowest price) if desired
            if (allResults.Any())
            {
                var minPrice = allResults.Min(r => r.Price);
                allResults.ForEach(r => r.IsBestDeal = r.Price == minPrice);
            }

            return allResults;
        }

        // Get products from Jumbo API
        private static async Task<List<ProductResult>> GetJumboProducts(string productName)
        {
            var url = $"{baseUrl}/jumbo/search?q={Uri.EscapeDataString(productName)}";
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<JumboProductResponse>>(url);
                if (response == null)
                    return new List<ProductResult>();

                return response.Select(jp => new ProductResult
                {
                    Store = "Jumbo",
                    // Assume price.amount is in cents; adjust if necessary.
                    Price = jp.product.data.prices.price.amount / 100.0,
                    ProductName = jp.product.data.title,
                    ProductDetails = jp.product.data.quantity,
                    ImageUrl = jp.product.data.imageInfo?.primaryView?.FirstOrDefault()?.url ?? string.Empty
                }).ToList();
            }
            catch (Exception)
            {
                // Log error if needed
                return new List<ProductResult>();
            }
        }

        // Get products from Aldi API
        private static async Task<List<ProductResult>> GetAldiProducts(string productName)
        {
            var url = $"{baseUrl}/aldi/search?q={Uri.EscapeDataString(productName)}";
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<AldiArticle>>(url);
                if (response == null)
                    return new List<ProductResult>();

                return response.Select(a => new ProductResult
                {
                    Store = "Aldi",
                    // Convert price from string to double (assumes price is in euros)
                    Price = double.TryParse(a.price, out var p) ? p : 0,
                    ProductName = a.title,
                    ProductDetails = a.articleId,
                    ImageUrl = a.primaryImage?.url ?? string.Empty
                }).ToList();
            }
            catch (Exception)
            {
                // Log error if needed
                return new List<ProductResult>();
            }
        }

        // Get products from Albert Heijn (AH) API
        private static async Task<List<ProductResult>> GetAHProducts(string productName)
        {
            var url = $"{baseUrl}/ah/search?q={Uri.EscapeDataString(productName)}";
            try
            {
                var response = await httpClient.GetFromJsonAsync<AHResponse>(url);
                if (response == null || response.products == null)
                    return new List<ProductResult>();

                return response.products.Select(ah => new ProductResult
                {
                    Store = "Albert Heijn",
                    Price = ah.priceV2.now,
                    ProductName = ah.title,
                    ProductDetails = ah.salesUnitSize,
                    // If AH returns an image URL, otherwise leave it empty.
                    ImageUrl = ah.imageUrl ?? string.Empty
                }).ToList();
            }
            catch (Exception)
            {
                // Log error if needed
                return new List<ProductResult>();
            }
        }
    }

    // --- DTO Classes for Jumbo API ---
    public class JumboProductResponse
    {
        public JumboProduct product { get; set; }
    }
    public class JumboProduct
    {
        public JumboProductData data { get; set; }
    }
    public class JumboProductData
    {
        public string title { get; set; }
        public string quantity { get; set; }
        public JumboPrices prices { get; set; }
        public JumboImageInfo imageInfo { get; set; }
    }
    public class JumboPrices
    {
        public JumboPrice price { get; set; }
    }
    public class JumboPrice
    {
        public double amount { get; set; }
        public string currency { get; set; }
    }
    public class JumboImageInfo
    {
        public List<JumboPrimaryView> primaryView { get; set; }
    }
    public class JumboPrimaryView
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    // --- DTO Classes for Aldi API ---
    public class AldiArticle
    {
        public string articleId { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string priceFormatted { get; set; }
        public AldiPrimaryImage primaryImage { get; set; }
    }
    public class AldiPrimaryImage
    {
        public string url { get; set; }
    }

    // --- DTO Classes for Albert Heijn (AH) API ---
    public class AHResponse
    {
        public List<AHProduct> products { get; set; }
    }
    public class AHProduct
    {
        public bool ageCheck { get; set; }
        public PriceV2 priceV2 { get; set; }
        public string title { get; set; }
        public string salesUnitSize { get; set; }
        // Optional: imageUrl if provided by the API.
        public string imageUrl { get; set; }
    }
    public class PriceV2
    {
        public double now { get; set; }
    }
}
