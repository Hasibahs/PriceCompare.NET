using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceComparisonAPI.Models;

namespace PriceComparisonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        // Helper to make sure Link has valid format
        private string EnsureValidUrl(string link, string supermarket)
        {
            if (string.IsNullOrWhiteSpace(link)) return "";

            if (link.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return link;

            if (supermarket.ToLower().Contains("jumbo"))
                return $"https://www.jumbo.com{link}";
            else if (supermarket.ToLower().Contains("ah"))
                return $"https://www.ah.nl{link}";
            else if (supermarket.ToLower().Contains("aldi"))
                return $"https://www.aldi.nl{link}";

            return link; // fallback
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _context.Products.ToListAsync();

            foreach (var p in products)
            {
                p.Link = EnsureValidUrl(p.Link, p.Supermarket);
            }

            return products.OrderBy(p => (double)p.Price).ToList();
        }

        // GET: api/products/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Link = EnsureValidUrl(product.Link, product.Supermarket);
            return product;
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product newProduct)
        {
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = newProduct.ProductId }, newProduct);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product updated)
        {
            if (id != updated.ProductId) return BadRequest("Product ID mismatch");

            _context.Entry(updated).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(p => p.ProductId == id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET: api/products/search?q=...
        // GET: api/products/search?q=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                var all = await _context.Products.ToListAsync();
                FixProductLinks(all);
                return all.OrderBy(p => (double)p.Price).ToList();
            }

            var query = q.Trim().ToLowerInvariant();

            var matched = await _context.Products
                .Where(p =>
                    p.Name.ToLower().Contains(query) ||
                    p.Supermarket.ToLower().Contains(query))
                .ToListAsync();

            FixProductLinks(matched);

            var sorted = matched
                .Select(p => new
                {
                    Product = p,
                    Score =
                        p.Name.ToLowerInvariant() == query ? 0 :
                        p.Name.ToLowerInvariant().Split(' ').Any(word => word == query) ? 1 :
                        p.Name.ToLowerInvariant().StartsWith(query) ? 2 :
                        p.Name.ToLowerInvariant().Contains(query) ? 3 :
                        p.Supermarket.ToLowerInvariant().Contains(query) ? 4 : 5
                })
                .OrderBy(p => p.Score)
                .ThenBy(p => (double)p.Product.Price)
                .Select(p => p.Product)
                .ToList();

            return sorted;
        }

        private void FixProductLinks(List<Product> products)
        {
            foreach (var p in products)
            {
                if (string.IsNullOrWhiteSpace(p.Link)) continue;

                // Jumbo
                if (p.Supermarket.ToLower().Contains("jumbo") && !p.Link.StartsWith("http"))
                {
                    if (!p.Link.StartsWith("/producten"))
                        p.Link = "https://www.jumbo.com/producten" + (p.Link.StartsWith("/") ? p.Link : "/" + p.Link);
                    else
                        p.Link = "https://www.jumbo.com" + p.Link;
                }

                // Albert Heijn
                else if (p.Supermarket.ToLower().Contains("ah") && !p.Link.StartsWith("http"))
                {
                    if (!p.Link.StartsWith("/producten"))
                        p.Link = "https://www.ah.nl/producten/product" + (p.Link.StartsWith("/") ? p.Link : "/" + p.Link);
                    else
                        p.Link = "https://www.ah.nl" + p.Link;
                }

                // Aldi
                else if (p.Supermarket.ToLower().Contains("aldi") && !p.Link.StartsWith("http"))
                {
                    if (!p.Link.StartsWith("/product"))
                        p.Link = "https://www.aldi.nl/product" + (p.Link.StartsWith("/") ? p.Link : "/" + p.Link);
                    else
                        p.Link = "https://www.aldi.nl" + p.Link;
                }
            }
        }


    }
}
