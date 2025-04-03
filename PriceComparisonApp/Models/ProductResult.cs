namespace PriceComparisonApp.Models
{
    public class ProductResult
    {
        public string Store { get; set; } = string.Empty;
        public double Price { get; set; }
        public bool IsBestDeal { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDetails { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
