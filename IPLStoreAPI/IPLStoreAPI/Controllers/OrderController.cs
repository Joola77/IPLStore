using IPLStoreAPI.Data;
using IPLStoreAPI.DTO;
using IPLStoreAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPLStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("PlaceOrder")]
        public async Task<ActionResult<OrderDto>> PlaceOrder()
        {
            var cartItems = await _context.CartItems.ToListAsync();
            if (cartItems.Count == 0) return BadRequest("Cart is empty");

            var order = new Order
            {
                Items = cartItems.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList(),
                TotalPrice = cartItems.Sum(i => i.Price * i.Quantity)
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Remove cart items after order creation
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        [HttpGet("OrderHistory")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            //return await _context.Orders.Include(o => o.Items).ToListAsync();

            var orders = await _context.Orders
            .Include(o => o.Items) // Fetch related OrderItems
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    ProductName = i.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            })
            .ToListAsync();

            return Ok(orders);
        }

    }
}
