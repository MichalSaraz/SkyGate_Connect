using Core.FlightContext.FlightInfo;
using Core.Interfaces;
using Core.PassengerContext.APIS;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;

        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Country> GetCountryByCriteriaAsync(Expression<Func<Country, bool>> criteria)
        {
            return await _context.Countries.AsNoTracking()
                .Where(criteria)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Country>> GetCountriesByCriteriaAsync(Expression<Func<Country, bool>> criteria)
        {
            return await _context.Countries.AsNoTracking()
                .Where(criteria)
                .ToListAsync();
        }
    }
}
