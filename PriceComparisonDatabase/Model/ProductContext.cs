using Microsoft.EntityFrameworkCore;

namespace PriceComparisonApp
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        private readonly string _dbPath;

        public ProductContext(string dbPath)
        {
            _dbPath = dbPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Points to a local SQLite file
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }
    }
}
