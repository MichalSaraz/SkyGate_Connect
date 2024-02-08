using Core.BaggageContext;
using Core.FlightContext;
using Core.FlightContext.FlightInfo;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Infrastructure.Repositories
{
    public class BaggageRepository : GenericRepository<Baggage>, IBaggageRepository
    {
        public BaggageRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Baggage> GetBaggageByCriteriaAsync(Expression<Func<Baggage, bool>> criteria)
        {
            return await _context.Baggage
                .Include(_ => _.Passenger)
                .Include(_ => _.BaggageTag)
                .Include(_ => _.SpecialBag)
                .Include(_ => _.FinalDestination)
                .Include(_ => _.Flights)
                .Where(criteria)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Baggage>> GetAllBaggageByCriteriaAsync(Expression<Func<Baggage, bool>> criteria)
        {
            return await _context.Baggage
                .Include(_ => _.Passenger)
                .Include(_ => _.BaggageTag)
                .Include(_ => _.SpecialBag)
                .Include(_ => _.FinalDestination)
                .Include(_ => _.Flights)
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

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT nextval('\"{sequenceName}\"')";
                var nextValue = cmd.ExecuteScalar();
                return Convert.ToInt32(nextValue);
            }
        }
    }
}
