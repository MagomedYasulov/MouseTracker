using Microsoft.EntityFrameworkCore;
using MouseTracker.Domain.Data;
using MouseTracker.Domain.Data.Entites;

namespace MouseTracker.Tests
{
    public class Common
    {
        public static void SeedData(ApplicationContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var random = new Random();
            if (!dbContext.Positions.Any())
            {
                var positions = new Position[200];
                for (var i = 0; i < 200; i++)
                {
                    positions[i] = new Position()
                    {
                        X = random.Next(0, 1920),
                        Y = random.Next(0, 1080),
                        MoveTime = DateTime.UtcNow + TimeSpan.FromSeconds(random.Next(-1000, 1000)),
                        CreatedAt = DateTime.UtcNow
                    };                  
                }
                dbContext.Positions.AddRange(positions);
                dbContext.SaveChanges();
            }
            DetachAllEntities(dbContext);
        }

        private static void DetachAllEntities(ApplicationContext dbContext)
        {
            var undetachedEntriesCopy = dbContext.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            foreach (var entry in undetachedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}
