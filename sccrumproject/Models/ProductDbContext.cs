using Microsoft.EntityFrameworkCore;

namespace sccrumproject.Models
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext()
        {
        }

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> products { get; set; }
    }
}
