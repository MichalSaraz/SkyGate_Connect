using Core.Interfaces;
using Core.SeatingContext;
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
    public class SeatRepository : GenericRepository<Seat>, ISeatRepository
    {
        public SeatRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Seat>> GetSeatsByCriteriaAsync(Expression<Func<Seat, bool>> criteria, bool tracked = true)
        {
            var seatQuery = _context.Seats.AsQueryable()
                .Include(_ => _.PassengerOrItem)
                    .ThenInclude(_ => _.Flights)
                .Where(criteria);

            if (!tracked)
            {
                seatQuery = seatQuery.AsNoTracking();
            }

            var seats = await seatQuery.ToListAsync();

            return seats;
        }

        public async Task<Seat> GetSeatByCriteriaAsync(Expression<Func<Seat, bool>> criteria)
        {
            return await _context.Seats.AsNoTracking()
                .Where(criteria)
                .FirstOrDefaultAsync();
        }
    }
}
