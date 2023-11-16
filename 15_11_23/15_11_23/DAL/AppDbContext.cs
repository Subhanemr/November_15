using _15_11_23.Models;
using Microsoft.EntityFrameworkCore;

namespace _15_11_23.DAL
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }


        public DbSet<Slide> Slides { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
