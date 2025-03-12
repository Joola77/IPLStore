using Microsoft.AspNetCore.Mvc;
using IPLStoreAPI.Models;
using Microsoft.EntityFrameworkCore;
using IPLStoreAPI.Data;
using IPLStoreAPI.DTO;

namespace IPLStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
        {
            return await _context.CartItems.ToListAsync();
        }

        [HttpPost("AddToCart")]
        public async Task<ActionResult<CartItem>> AddToCart(CartRequest request)
        {
            // Find the product by name
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == request.Name);

            if (product == null)
            {
                return NotFound($"Product '{request.Name}' not found.");
            }

            // Check if the item is already in the cart
            var existingItem = await _context.CartItems.FirstOrDefaultAsync(c => c.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = request.Quantity
                };
                _context.CartItems.Add(newItem);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Item added to cart successfully!" });
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
