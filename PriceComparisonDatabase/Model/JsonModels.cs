using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace PriceComparisonApp
{
    // Represents one top-level supermarket object from your JSON
    public class Root
    {
        [JsonPropertyName("n")]
        public string SupermarketName { get; set; } = default!;

        [JsonPropertyName("d")]
        public List<SupermarketProduct> Products { get; set; } = new();
    }

    // Represents one product object inside the "d" array
    public class SupermarketProduct
    {
        [JsonPropertyName("n")]
        public string ProductName { get; set; } = default!;

        [JsonPropertyName("l")]
        public string Link { get; set; } = default!;

        [JsonPropertyName("p")]
        public decimal Price { get; set; }

        [JsonPropertyName("s")]
        public string Size { get; set; } = default!;
    }
}
