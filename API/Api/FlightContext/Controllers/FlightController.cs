using API.Api.FlightContext.Models;
using API.Errors;
using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.JoinClasses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace API.Api.FlightContext.Controllers
{
    [ApiController]
    [Route("flight")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly IMapper _mapper;

        public FlightController(
            IFlightRepository flightRepository,
            ITimeProvider timeProvider,
            IPassengerRepository passengerRepository,
            IMapper mapper)
        {
            _flightRepository = flightRepository;
            _timeProvider = timeProvider;
            _passengerRepository = passengerRepository;
            _mapper = mapper;
        }

        [HttpPost("search")]
        public async Task<ActionResult<List<Flight>>> SearchFlights([FromBody] JObject data)
        {
            var model = new FlightSearchModel
            {
                DepartureDate = _timeProvider.ParseDate(data["departureDate"]?.ToString()),
                AirlineId = data["airlineId"]?.ToString(),
                DestinationFrom = data["destinationFrom"]?.ToString(),
                DestinationTo = data["destinationTo"]?.ToString(),
                FlightNumber = data["flightNumber"]?.ToString()
            };

            if (!string.IsNullOrEmpty(model.FlightNumber) &&
                string.IsNullOrEmpty(model.AirlineId))
            {
                return BadRequest(new ApiResponse(400, "AirlineId must be specified when searching by flight number."));
            }

            if (model == null ||
               (!model.DepartureDate.HasValue &&
                string.IsNullOrEmpty(model.AirlineId) &&
                string.IsNullOrEmpty(model.DestinationFrom) &&
                string.IsNullOrEmpty(model.DestinationTo)))
            {
                return BadRequest(new ApiResponse(400, "At least one field must be filled in for the search criteria."));
            }

            Expression<Func<Flight, bool>> criteria = f =>
                (!model.DepartureDate.HasValue ||
                    f.DepartureDateTime.Date == model.DepartureDate.Value.Date) &&
                (string.IsNullOrEmpty(model.AirlineId) ||
                    f.ScheduledFlight.AirlineId == model.AirlineId) &&
                (string.IsNullOrEmpty(model.DestinationFrom) ||
                    f.ScheduledFlight.DestinationFromId == model.DestinationFrom) &&
                (string.IsNullOrEmpty(model.DestinationTo) ||
                    f.ScheduledFlight.DestinationToId == model.DestinationTo) &&
                (string.IsNullOrEmpty(model.FlightNumber) ||
                    f.ScheduledFlightId.Substring(2) == model.FlightNumber);

            var flights = await _flightRepository.GetFlightsByCriteriaAsync(criteria);

            var flightDtos = _mapper.Map<List<FlightDetailsDto>>(flights);

            if (flightDtos.Count == 0)
            {
                return NotFound(new ApiResponse(404, "No results found matching the specified criteria."));
            }

            return Ok(flightDtos);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<Flight>> GetFlightDetails(int id)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(id, false);

            var flightDto = _mapper.Map<FlightDetailsDto>(flight);

            if (flightDto == null)
            {
                return NotFound(new ApiResponse(404, $"Flight {id} not found"));
            }

            return Ok(flightDto);
        }

        [HttpGet("{id}/onward-flights")]
        public async Task<ActionResult<List<Flight>>> GetOnwardFlights(int id)
        {
            return await GetConnectedFlights(id, f =>
                f.IteratedFlight.DepartureDateTime > f.CurrentFlight.DepartureDateTime);
        }

        [HttpGet("{id}/inbound-flights")]
        public async Task<ActionResult<List<Flight>>> GetInboundFlights(int id)
        {
            return await GetConnectedFlights(id, f =>
                f.IteratedFlight.DepartureDateTime < f.CurrentFlight.DepartureDateTime);
        }

        private async Task<ActionResult<List<Flight>>> GetConnectedFlights(int id,
            Func<(int FlightId, Flight IteratedFlight, Flight CurrentFlight), bool> condition)
        {
            var currentFlight = await _flightRepository.GetFlightByIdAsync(id, false);

            if (currentFlight == null)
            {
                return NotFound(new ApiResponse(404, $"Flight {id} not found"));
            }

            var connectedFlights = new List<Flight>();

            foreach (var passengerFlight in currentFlight.ListOfBookedPassengers)
            {
                var passenger = await _passengerRepository
                    .GetPassengerByIdAsync(passengerFlight.PassengerId);

                var matchingFlights = passenger.Flights
                    .Where(f =>
                        f.FlightId != currentFlight.Id &&
                        condition((f.FlightId, f.Flight, currentFlight)))
                    .Select(f => f.FlightId);

                foreach (var otherFlightId in matchingFlights)
                {
                    var otherFlight = await _flightRepository.GetFlightByIdAsync(otherFlightId, false);

                    if (otherFlight != null)
                    {
                        connectedFlights.Add(otherFlight);
                    }
                }
            }

            var flightCounts = connectedFlights
                .GroupBy(f => f.Id)
                .ToDictionary(group => group.Key, group => group.Count());

            var flightDtos = _mapper.Map<List<FlightConnectionsDto>>(connectedFlights)
                .Select(f =>
                {
                    f.Count = flightCounts.ContainsKey(f.Id) ? flightCounts[f.Id] : 0;
                    return f;
                })
                .DistinctBy(f => f.Id)
                .ToList();

            if (flightDtos.Count == 0)
            {
                return NotFound(new ApiResponse(404, "No results found matching the specified criteria."));
            }

            return Ok(flightDtos);
        }

        [HttpGet("{id}/passengers-with-onward-flight")]
        public async Task<ActionResult<List<Passenger>>> GetPassengersWithOnwardFlight(int id)
        {
            return await _GetPassengersWithFlightConnection(id, true);
        }

        [HttpGet("{id}/passengers-with-inbound-flight")]
        public async Task<ActionResult<List<Passenger>>> GetPassengersWithInboundFlight(int id)
        {
            return await _GetPassengersWithFlightConnection(id, false);
        }

        private async Task<ActionResult<List<Passenger>>> _GetPassengersWithFlightConnection(int id, bool isOnwardFlight)
        {
            var passengers = await _passengerRepository.GetPassengersWithFlightConnectionsAsync(id, isOnwardFlight);

            var passengerDtos = _mapper.Map<List<PassengerDetailsDto>>(passengers, opt =>
            {
                opt.Items["DepartureDateTime"] = passengers
                    .FirstOrDefault().Flights
                        .FirstOrDefault(f => f.FlightId == id).Flight.DepartureDateTime;
                opt.Items["FlightId"] = id;
            })
                .Select(s => new
                {
                    s.FirstName,
                    s.LastName,
                    s.Gender,
                    s.NumberOfCheckedBags,
                    s.SeatOnCurrentFlight?.SeatNumber,
                    s.SeatOnCurrentFlight?.FlightClass,
                    FlightDetails = isOnwardFlight ? s.ConnectingFlights : s.InboundFlights
                });

            return Ok(passengerDtos);
        }
    }
}
