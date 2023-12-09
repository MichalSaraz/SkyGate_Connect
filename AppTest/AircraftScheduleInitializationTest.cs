using Core.Time;
using Infrastructure.Data;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace AppTest2
{
    public class AircraftScheduleInitializationTest
    {
        [Fact]
        public void InitializeAircraftSchedule()
        {
            // Načtení connection string z appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            // Vytvoření instance DbContextOptions s použitím connection string
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(config.GetConnectionString("DefaultConnection"))
                .Options;

            using (var dbContext = new AppDbContext(options, config))
            {
                // Vytvoření instance vaší služby s testovacím `testTimeProvider` a `dbContext`
                var service = new AircraftScheduleInitialization(dbContext);

                // Spuštění metody pro vytvoření letů pro příštích 7 dní
                service.PlanFlights();
            }
        }
    }
}
