using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class APISDataInitializationTest
    {
        [Fact]
        public void InitializeAPISData()
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
                var service = new APISDataInitialization(dbContext);

                // Spuštění metody pro vytvoření letů pro příštích 7 dní
                service.GenerateAPIS();
            }
        }
    }
}
