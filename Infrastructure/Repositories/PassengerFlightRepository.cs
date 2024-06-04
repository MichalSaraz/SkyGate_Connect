using Core.FlightContext.FlightInfo;
using Core.Interfaces;
using Core.PassengerContext.JoinClasses;
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
    public class PassengerFlightRepository : IPassengerFlightRepository
    {
        private readonly AppDbContext _context;

        public PassengerFlightRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetHighestSequenceNumberOfTheFlight(Guid flightId)
        {
            var passengersOfTheFlight = _context.PassengerFlight
                .AsQueryable()
                .AsNoTracking()
                .Where(_ => _.FlightId == flightId);

            var highestSequenceNumber = await passengersOfTheFlight.MaxAsync(_ => _.BoardingSequenceNumber) ?? 0;

            return highestSequenceNumber;
        }
    }
}
