using Core.FlightContext;
using Core.Interfaces;
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
    //public class ScheduledFlightRepository : IScheduledFlightRepository
    //{
    //    private readonly AppDbContext _context;

    //    public ScheduledFlightRepository(AppDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<ScheduledFlight> GetScheduledFlightByIdAsync(string id)
    //    {
    //        return await _context.ScheduledFlights.AsNoTracking()
    //            .Where(string.IsNullOrEmpty(id) ? _ => false : _ => _.FlightNumber == id)
    //            .FirstOrDefaultAsync();
    //    }
    //}
}
