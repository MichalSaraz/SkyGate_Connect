using Core.FlightContext.FlightInfo;
using Core.SeatingContext;
using System.Text.Json;
using Core.PassengerContext.APIS;

namespace Infrastructure.Data
{
    public abstract class AppDbSeedData
    {
        /// <summary>
        /// Seeds the database with initial data for all entities.
        /// </summary>
        /// <param name="context">The instance of <see cref="AppDbContext"/>.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedAll(AppDbContext context)
        {
            await _SeedAsync<Country>(context, "../Infrastructure/Data/SeedData/Countries.json");
            await _SeedAsync<Destination>(context, "../Infrastructure/Data/SeedData/Destinations.json");
            await _SeedAsync<Airline>(context, "../Infrastructure/Data/SeedData/Airlines.json");
            await _SeedAsync<AircraftType>(context, "../Infrastructure/Data/SeedData/AircraftTypes.json");
            await _SeedAsync<Aircraft>(context, "../Infrastructure/Data/SeedData/Aircrafts.json");
            await _SeedAsync<SeatMap>(context, "../Infrastructure/Data/SeedData/SeatMaps.json");
        }

        /// <summary>
        /// Seed the database with the specified type of entity using the provided JSON file.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to seed.</typeparam>
        /// <param name="context">The <see cref="AppDbContext"/> instance.</param>
        /// <param name="jsonFilePath">The path to the JSON file containing the entities.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static async Task _SeedAsync<TEntity>(AppDbContext context, string jsonFilePath) where TEntity : class
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
