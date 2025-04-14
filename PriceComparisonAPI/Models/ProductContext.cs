using Microsoft.EntityFrameworkCore;



namespace PriceComparisonAPI.Models
{
    public class ProductContext : DbContext
    {
        // A DbSet for your Product entity
        public DbSet<Product> Products => Set<Product>();

        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {

        }
    }
}
