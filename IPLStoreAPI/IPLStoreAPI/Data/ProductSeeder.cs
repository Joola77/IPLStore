using IPLStoreAPI.Models;

namespace IPLStoreAPI.Data
{
    public class ProductSeeder
    {

        public static void SeedProducts(AppDbContext context)
        {
            if (!context.Products.Any()) // Check if there are existing products
            {
                var products = new List<Product>
            {
                new Product { Name = "CSK Hoodie", Description = "Official CSK Hoodie", Category = "Clothing", Price = 1299, Franchise = "CSK", StockQuantity = 50, ImageUrl = "/images/csk-hoodie.jpg" },
                new Product { Name = "MI Wristband", Description = "MI Supporters Wristband", Category = "Accessories", Price = 199, Franchise = "MI", StockQuantity = 100, ImageUrl = "/images/mi-wristband.jpg" },
                new Product { Name = "RCB Mug", Description = "RCB Coffee Mug", Category = "Mug", Price = 499, Franchise = "RCB", StockQuantity = 30, ImageUrl = "/images/rcb-mug.jpg" },
                new Product { Name = "KKR Keychain", Description = "KKR Official Keychain", Category = "Accessories", Price = 150, Franchise = "KKR", StockQuantity = 80, ImageUrl = "/images/kkr-keychain.jpg" },
                new Product { Name = "SRH Cap", Description = "Sunrisers Hyderabad Cap", Category = "Cap", Price = 399, Franchise = "SRH", StockQuantity = 40, ImageUrl = "/images/srh-cap.jpg" }
            };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }

}
}
