using Microsoft.EntityFrameworkCore;
using MouseTracker.Domain.Data.Entites;

namespace MouseTracker.Domain.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Position> Positions => Set<Position>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
