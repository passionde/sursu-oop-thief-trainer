using Microsoft.EntityFrameworkCore;

namespace ThiefHackUI
{
    public class AppDbContext : DbContext
    {
        public DbSet<Offset> Offset { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    }
}
