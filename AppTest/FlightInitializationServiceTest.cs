using Core.Time;
using Infrastructure.Data;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AppTest
{
    public class FlightInitializationServiceTest
    {
        [Fact]
        public void InitializeFlights()
        {
            // Naètení connection string z appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            // Vytvoøení instance DbContextOptions s použitím connection string
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(config.GetConnectionString("DefaultConnection"))
                .Options;

            using (var dbContext = new AppDbContext(options, config))
            {
                var testTimeProvider = new TestTimeProvider(new DateTime(2023, 10, 1)); // Simulovaný èas pro test

                // Nastavení simulovaného èasu na jiný èas
                testTimeProvider.SetSimulatedTime(new DateTime(2023, 10, 15));

                // Vytvoøení instance vaší služby s testovacím `testTimeProvider` a `dbContext`
                var service = new FlightInitializationService(dbContext, testTimeProvider);

                // Spuštìní metody pro vytvoøení letù pro pøíštích 7 dní
                service.CreateFlightsForNext7Days();                
            }
        }
    }
}