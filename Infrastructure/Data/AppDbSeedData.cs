using Core.FlightContext.FlightInfo;
using Core.PassengerContext.Regulatory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbSeedData
    {
        public static async Task SeedAll(AppDbContext context)
        {
            await SeedAsync<Country>(context, "../Infrastructure/Data/SeedData/Countries.json");
            await SeedAsync<Destination>(context, "../Infrastructure/Data/SeedData/Destination.json");
            await SeedAsync<Airline>(context, "../Infrastructure/Data/SeedData/Airlines.json");
            await SeedAsync<AircraftType>(context, "../Infrastructure/Data/SeedData/AircraftType.json");
            await SeedAsync<Aircraft>(context, "../Infrastructure/Data/SeedData/Aircrafts.json");
        }

        public static async Task SeedAsync<TEntity>(AppDbContext context, string jsonFilePath) where TEntity : class
        {
            if (!context.Set<TEntity>().Any())
            {
                var jsonData = File.ReadAllText(jsonFilePath);
                var entities = JsonSerializer.Deserialize<List<TEntity>>(jsonData);
                context.Set<TEntity>().AddRange(entities);
                await context.SaveChangesAsync();
            }
        }
    }
}
