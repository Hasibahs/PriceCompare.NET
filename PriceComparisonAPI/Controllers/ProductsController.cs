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

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            // Fix: move data to memory before sorting to avoid SQLite decimal bug
            return (await _context.Products.ToListAsync())
                .OrderBy(p => (double)p.Price)
                .ToList();
        }

        // GET: api/products/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
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
            return NoContent(); // 204
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent(); // 204
        }

        // GET: api/products/search?q=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return (await _context.Products.ToListAsync())
                    .OrderBy(p => (double)p.Price)
                    .ToList();
            }

            var query = q.Trim().ToLowerInvariant();

            var matched = await _context.Products
                .Where(p =>
                    p.Name.ToLower().Contains(query) || p.Supermarket.ToLower().Contains(query))
                .ToListAsync();

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
    }
}
