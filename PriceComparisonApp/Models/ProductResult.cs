using System.Windows.Input;

namespace PriceComparisonApp.Models
{
    public class ProductResult
    {
        public string Store { get; set; } = string.Empty;
        public double Price { get; set; }
        public bool IsBestDeal { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDetails { get; set; } = string.Empty;

        // ✅ This now holds the clickable product URL
        public string Link { get; set; } = string.Empty;

        // Optional: command to open product link (can be used in other views)
        public ICommand OpenProductLinkCommand => new Command<string>((url) =>
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                try
                {
                    Launcher.Default.OpenAsync(new Uri(url));
                }
                catch (Exception)
                {
                    // You can log the error if needed
                }
            }
        });
    }
}
