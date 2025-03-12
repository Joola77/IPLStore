using Microsoft.AspNetCore.Mvc;
using IPLStoreAPI.Data;
using IPLStoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using IPLStoreAPI.DTO;

namespace IPLStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
            string? name,
            string? category,
            string? franchise)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Contains(category));
            }

            if (!string.IsNullOrEmpty(franchise))
            {
                query = query.Where(p => p.Franchise.Contains(franchise));
            }

            var products = await query.ToListAsync();
            return Ok(products);
        }


        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPost("add")]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
    }
}
