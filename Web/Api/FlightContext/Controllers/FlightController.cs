using System.Linq.Expressions;
using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.Interfaces;
using Core.PassengerContext;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Web.Api.FlightContext.Models;
using Web.Errors;

namespace Web.Api.FlightContext.Controllers
{
    [ApiController]
    [Route("flight")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IBaseFlightRepository _baseFlightRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly IMapper _mapper;

        public FlightController(
            IFlightRepository flightRepository,
            IBaseFlightRepository baseFlightRepository,
            IPassengerRepository passengerRepository,
            ITimeProvider timeProvider,
            IMapper mapper)
        {
            _flightRepository = flightRepository;
            _baseFlightRepository = baseFlightRepository;
            _passengerRepository = passengerRepository;
            _timeProvider = timeProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// Searches for flights based on the provided search criteria.
        /// </summary>
        /// <param name="data">The search criteria for the flight search.</param>
        /// <returns>A list of flights that match the search criteria.</returns>
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
                return BadRequest(new ApiResponse(
                    400, "AirlineId must be specified when searching by flight number."));
            }

            if (!model.DepartureDate.HasValue &&
                 string.IsNullOrEmpty(model.AirlineId) &&
                 string.IsNullOrEmpty(model.DestinationFrom) &&
                 string.IsNullOrEmpty(model.DestinationTo))
            {
                return BadRequest(new ApiResponse(
                    400, "At least one field must be filled in for the search criteria."));
            }

            Expression<Func<Flight, bool>> criteria = c =>
                (!model.DepartureDate.HasValue ||
                    c.DepartureDateTime.Date == model.DepartureDate.Value.Date) &&
                (string.IsNullOrEmpty(model.AirlineId) ||
                    c.AirlineId == model.AirlineId) &&
                (string.IsNullOrEmpty(model.DestinationFrom) ||
                    c.DestinationFromId == model.DestinationFrom) &&
                (string.IsNullOrEmpty(model.DestinationTo) ||
                    c.DestinationToId == model.DestinationTo) &&
                (string.IsNullOrEmpty(model.FlightNumber) ||
                    c.ScheduledFlightId.Substring(2) == model.FlightNumber);

            var flights = await _flightRepository.GetFlightsByCriteriaAsync(criteria);

            var flightDtos = _mapper.Map<List<FlightDetailsDto>>(flights);

            if (flightDtos.Count == 0)
            {
                return NotFound(new ApiResponse(404, "No results found matching the specified criteria."));
            }

            return Ok(flightDtos);
        }

        /// <summary>
        /// Retrieves the details of a flight.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>The flight details.</returns>
        [HttpGet("{id:int}/details")]
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

        /// <summary>
        /// Get the onward flights for a specific flight.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>The list of onward flights as a <see cref="List{T}"/> of <see cref="BaseFlight"/>.</returns>
        [HttpGet("{id:int}/onward-flights")]
        public async Task<ActionResult<List<BaseFlight>>> GetOnwardFlights(int id)
        {
            return await _GetConnectedFlights(id, bf =>
                bf.IteratedFlight.DepartureDateTime > bf.CurrentFlight.DepartureDateTime);
        }

        /// <summary>
        /// Retrieves a list of inbound flights related to a specific flight.
        /// </summary>
        /// <param name="id">The ID of the flight to retrieve inbound flights for.</param>
        /// <returns>A list of <see cref="BaseFlight"/> objects representing the inbound flights.</returns>
        [HttpGet("{id:int}/inbound-flights")]
        public async Task<ActionResult<List<BaseFlight>>> GetInboundFlights(int id)
        {
            return await _GetConnectedFlights(id, bf =>
                bf.IteratedFlight.DepartureDateTime < bf.CurrentFlight.DepartureDateTime);
        }

        private async Task<ActionResult<List<BaseFlight>>> _GetConnectedFlights(int id,
            Func<(int FlightId, BaseFlight IteratedFlight, BaseFlight CurrentFlight), bool> condition)
        {
            var currentFlight = await _flightRepository.GetFlightByIdAsync(id, false);

            if (currentFlight == null)
            {
                return NotFound(new ApiResponse(404, $"Flight {id} not found"));
            }

            var connectedFlights = new List<BaseFlight>();

            foreach (var passengerFlight in currentFlight.ListOfBookedPassengers)
            {
                var passenger = await _passengerRepository
                    .GetPassengerByIdAsync(passengerFlight.PassengerOrItemId);

                var nonCurrentFlightsId = passenger.Flights
                    .Where(pf =>
                        pf.FlightId != currentFlight.Id &&
                        condition((pf.FlightId, pf.Flight, currentFlight)))
                    .Select(pf => pf.FlightId);

                foreach (var nonCurrentFlightId in nonCurrentFlightsId)
                {
                    var nonCurrentFlight = await _baseFlightRepository.GetFlightByIdAsync(nonCurrentFlightId, false);

                    if (nonCurrentFlight != null)
                    {
                        connectedFlights.Add(nonCurrentFlight);
                    }
                }
            }

            var flightCounts = connectedFlights
                .GroupBy(f => f.Id)
                .ToDictionary(group => group.Key, group => group.Count());

            var flightDtos = _mapper.Map<List<FlightConnectionsDto>>(connectedFlights)
                .Select(f =>
                {
                    f.Count = flightCounts.GetValueOrDefault(f.Id, 0);
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

        /// <summary>
        /// Gets the list of passengers with onward flight for a given flight ID.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>The list of passengers with onward flight.</returns>
        [HttpGet("{id:int}/passengers-with-onward-flight")]
        public async Task<ActionResult<List<Passenger>>> GetPassengersWithOnwardFlight(int id)
        {
            return await _GetPassengersWithFlightConnection(id, true);
        }

        /// <summary>
        /// Retrieves a list of passengers with an inbound flight for the specified flight ID.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>A list of passengers with an inbound flight.</returns>
        [HttpGet("{id:int}/passengers-with-inbound-flight")]
        public async Task<ActionResult<List<Passenger>>> GetPassengersWithInboundFlight(int id)
        {
            return await _GetPassengersWithFlightConnection(id, false);
        }

        private async Task<ActionResult<List<Passenger>>> _GetPassengersWithFlightConnection(int id, 
            bool isOnwardFlight)
        {
            var passengers = await _passengerRepository.GetPassengersWithFlightConnectionsAsync(id, isOnwardFlight);

            var passengerDtos = _mapper.Map<List<PassengerDetailsDto>>(passengers, opt =>
            {
                opt.Items["DepartureDateTime"] = passengers
                    .FirstOrDefault()?.Flights
                        .FirstOrDefault(pf => pf.FlightId == id)?.Flight.DepartureDateTime;
                opt.Items["FlightId"] = id;
            })
                .Select(pdd => new
                {
                    pdd.FirstName,
                    pdd.LastName,
                    pdd.Gender,
                    pdd.NumberOfCheckedBags,
                    pdd.SeatOnCurrentFlight?.SeatNumber,
                    pdd.SeatOnCurrentFlight?.FlightClass,
                    FlightDetails = isOnwardFlight ? pdd.ConnectingFlights : pdd.InboundFlights
                });

            return Ok(passengerDtos);
        }
    }
}
