using CareerApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

namespace TopicosP1Backend.Scripts
{
    public class TestContext : Context
    {
        public TestContext(DbContextOptions<Context> options)
        : base(options)
        {
        }
    }
    public static class TestExtensions
    {
        public static void CreateTestDbIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<TestContext>();
            context.Database.EnsureCreated();
            DatabaseInitialization.Populate(context);
        }
    }
}
