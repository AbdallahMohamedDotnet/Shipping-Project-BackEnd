using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class ShippingContextFactory : IDesignTimeDbContextFactory<ShippingContext>
    {
        public ShippingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShippingContext>();
            
            // Use the connection string directly for design-time operations
            optionsBuilder.UseSqlServer("Server=abdallah;Database=Shipping;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;");

            return new ShippingContext(optionsBuilder.Options);
        }
    }
}