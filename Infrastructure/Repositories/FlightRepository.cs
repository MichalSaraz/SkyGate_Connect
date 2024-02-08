using Core.FlightContext;
using Core.FlightContext.FlightInfo;
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
    public class FlightRepository : IFlightRepository
    {
        private readonly AppDbContext _context;

        public FlightRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Flight> GetFlightByCriteriaAsync(Expression<Func<Flight, bool>> criteria)
        {
            return await _context.Flights
                .Include(_ => _.ScheduledFlight)
                    .ThenInclude(_ => _.DestinationFrom)
                .Include(_ => _.ScheduledFlight)
                    .ThenInclude(_ => _.DestinationTo)
                .Include(_ => _.ScheduledFlight)
                    .ThenInclude(_ => _.Airline)
                .Include(_ => _.Aircraft)
                    .ThenInclude(_ => _.AircraftType)
                .Include(_ => _.Boarding)
                .Include(_ => _.ListOfBookedPassengers)
                    .ThenInclude(_ => _.Passenger)
                .Include(_ => _.ListOfCheckedBaggage)
                    .ThenInclude(_ => _.Baggage)
                .Include(_ => _.Seats)
                .Where(criteria)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Flight>> GetFlightsByCriteriaAsync(Expression<Func<Flight, bool>> criteria)
        {
            return await _context.Flights
                .Include(_ => _.ScheduledFlight)
                    .ThenInclude(_ => _.DestinationFrom)
                .Include(_ => _.ScheduledFlight)
                    .ThenInclude(_ => _.DestinationTo)
                .Include(_ => _.ScheduledFlight)
                    .ThenInclude(_ => _.Airline)
                .Include(_ => _.Aircraft)
                    .ThenInclude(_ => _.AircraftType)
                .Include(_ => _.Boarding)
                .Include(_ => _.ListOfBookedPassengers)
                    .ThenInclude(_ => _.Passenger)
                .Include(_ => _.ListOfCheckedBaggage)
                    .ThenInclude(_ => _.Baggage)
                .Include(_ => _.Seats)
                .Where(criteria)
                .ToListAsync();
        }
    }
}
