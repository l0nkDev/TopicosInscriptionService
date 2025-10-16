using Microsoft.EntityFrameworkCore;
using TopicosP1Backend.Scripts;

namespace TopicosP1Backend.Cache
{
    public class CacheContext: DbContext
    {
        public CacheContext(DbContextOptions<CacheContext> options)
        : base(options)
        {
        }

        public DbSet<QueuedFunction.DBItem> QueuedFunctions { get; set; }
        public DbSet<Qcount> qcounts { get; set; }
        public DbSet<wcount> wcounts { get; set; }
    }

    public static class Extensions
    {
        public static void CreateCacheIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<CacheContext>();
            context.Database.EnsureCreated();
        }
    }
}
