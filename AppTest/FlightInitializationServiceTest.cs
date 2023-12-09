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
            // Na�ten� connection string z appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            // Vytvo�en� instance DbContextOptions s pou�it�m connection string
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(config.GetConnectionString("DefaultConnection"))
                .Options;

            using (var dbContext = new AppDbContext(options, config))
            {
                var testTimeProvider = new TestTimeProvider(new DateTime(2023, 10, 1)); // Simulovan� �as pro test

                // Nastaven� simulovan�ho �asu na jin� �as
                testTimeProvider.SetSimulatedTime(new DateTime(2023, 10, 15));

                // Vytvo�en� instance va�� slu�by s testovac�m `testTimeProvider` a `dbContext`
                var service = new FlightInitializationService(dbContext, testTimeProvider);

                // Spu�t�n� metody pro vytvo�en� let� pro p��t�ch 7 dn�
                service.CreateFlightsForNext7Days();                
            }
        }
    }
}