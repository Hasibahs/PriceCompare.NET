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

        // This holds the product URL
        public string ImageUrl { get; set; } = string.Empty;

        // Command to open the product link in the browser
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
