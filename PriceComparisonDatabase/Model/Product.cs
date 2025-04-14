using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceComparisonApp
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public string Supermarket { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Size { get; set; } = default!;
        public decimal Price { get; set; }
        public string Link { get; set; } = default!;

    }
}
