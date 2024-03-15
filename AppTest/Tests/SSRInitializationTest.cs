using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestProject.TestDataInitializationClasses;

namespace TestProject.Tests
{
    public class SSRInitializationTest
    {
        [Fact]
        public void GeneratePassengerItinerary()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(config.GetConnectionString("DefaultConnection"))
                .Options;

            using var dbContext = new AppDbContext(options, config);
            var service = new SSRInitialization(dbContext);

            service.InitializeSSR();
        }
    }
}







