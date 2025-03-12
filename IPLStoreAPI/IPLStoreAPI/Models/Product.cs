using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IPLStoreAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; } // Jersey, Cap, Flag, etc.
        public decimal Price { get; set; }
        public string Franchise { get; set; } // Example: "CSK", "MI", "RCB"
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
    }

}
