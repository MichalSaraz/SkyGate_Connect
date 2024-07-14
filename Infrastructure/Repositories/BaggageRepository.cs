using Core.BaggageContext;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Data;

namespace Infrastructure.Repositories
{
    public class BaggageRepository : GenericRepository<Baggage>, IBaggageRepository
    {
        public BaggageRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Baggage> GetBaggageByTagNumber(string tagNumber)
        {
            return await _context.Baggage.AsNoTracking()
                .Include(_ => _.Passenger)
                .Include(_ => _.BaggageTag)
                .Include(_ => _.SpecialBag)
                .Include(_ => _.FinalDestination)
                .Include(_ => _.Flights)
                    .ThenInclude(_ => _.Flight)
                .FirstOrDefaultAsync(_ => _.BaggageTag.TagNumber == tagNumber);
        }

        public async Task<Baggage> GetBaggageByCriteriaAsync(Expression<Func<Baggage, bool>> criteria,
            bool tracked = true)
        {
            var baggageQuery = _context.Baggage.AsQueryable()
                .Include(_ => _.Passenger)
                .Include(_ => _.BaggageTag)
                .Include(_ => _.SpecialBag)
                .Include(_ => _.Flights)
                .Include(_ => _.FinalDestination)
                .Where(criteria);
            
            if (!tracked)
            {
                baggageQuery = baggageQuery.AsNoTracking();
            }
            
            var baggage = await baggageQuery.FirstOrDefaultAsync();
            
            return baggage;
        }

        public async Task<IReadOnlyList<Baggage>> GetAllBaggageByCriteriaAsync(Expression<Func<Baggage, bool>> criteria)
        {
            return await _context.Baggage.AsNoTracking()
                .Include(_ => _.Passenger)
                .Include(_ => _.BaggageTag)
                .Include(_ => _.SpecialBag)
                .Include(_ => _.FinalDestination)
                .Include(_ => _.Flights)
                    .ThenInclude(_ => _.Flight)
                .Where(criteria)
                .ToListAsync();
        }

        public int GetNextSequenceValue(string sequenceName)
        {
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT nextval('\"{sequenceName}\"')";
            var nextValue = cmd.ExecuteScalar();
            return Convert.ToInt32(nextValue);
        }
    }
}