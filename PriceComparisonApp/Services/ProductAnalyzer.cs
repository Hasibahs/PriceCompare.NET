using PriceComparisonApp.Models;

namespace PriceComparisonApp.Services
{
    public static class ProductAnalyzer
    {
        public static Task<List<StorePriceSummary>> AnalyzeAsync(List<ProductResult> products)
        {
            return Task.Run(() =>
            {
                var result = products
                    .AsParallel()
                    .GroupBy(p => p.Store)
                    .Select(g => new StorePriceSummary
                    {
                        Store = g.Key,
                        MinPrice = g.Min(p => p.Price),
                        MaxPrice = g.Max(p => p.Price),
                        AvgPrice = g.Average(p => p.Price),
                        Count = g.Count()
                    })
                    .ToList();

                return result;
            });
        }
    }

    public class StorePriceSummary
    {
        public string Store { get; set; } = "";
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public double AvgPrice { get; set; }
        public int Count { get; set; }
    }
}
