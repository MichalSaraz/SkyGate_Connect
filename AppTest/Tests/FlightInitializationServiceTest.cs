using Core.Time;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestProject.TestDataInitializationClasses;

namespace TestProject.Tests
{
    public class FlightInitializationServiceTest
    {
        [Fact]
        public void InitializeFlights()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(config.GetConnectionString("DefaultConnection"))
                .Options;

            using var dbContext = new AppDbContext(options, config);
            var testTimeProvider = new TestTimeProvider(/*new DateTime(2023, 10, 1)*/);

            //testTimeProvider.SetSimulatedTime(new DateTime(2023, 10, 15));

            var service = new FlightInitializationService(dbContext, testTimeProvider);

            service.CreateFlightsForNext7Days();
        }
    }
}