using Microsoft.EntityFrameworkCore;
using PriceComparisonAPI.Models; // We'll define these in the Models folder

var builder = WebApplication.CreateBuilder(args);

// Register DbContext for DI, telling EF to use "products.db"
builder.Services.AddDbContext<ProductContext>(options =>
{
    options.UseSqlite("Data Source=products.db");
});

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();



// Map all controller endpoints (i.e. /api/products)
app.MapControllers();

// Run the app
app.Run();
