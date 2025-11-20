using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NineCafeProductAppV1.Models;

namespace NineCafeProductAppV1.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<ProductPosting> productPostings { get; set; }
    }
}
