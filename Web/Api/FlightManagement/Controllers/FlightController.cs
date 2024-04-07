using System.Linq.Expressions;
using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.JoinClasses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Web.Api.FlightManagement.Models;
using Web.Errors;

namespace Web.Api.FlightManagement.Controllers
{
    [ApiController]
    [Route("flight-management")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IBaseFlightRepository _baseFlightRepository;
        private readonly IOtherFlightRepository _otherFlightRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly IMapper _mapper;

        public FlightController(
            IFlightRepository flightRepository,
            IBaseFlightRepository baseFlightRepository,
            IOtherFlightRepository otherFlightRepository,
            IPassengerRepository passengerRepository,
            ITimeProvider timeProvider,
            IMapper mapper)
        {
            _flightRepository = flightRepository;
            _baseFlightRepository = baseFlightRepository;
            _otherFlightRepository = otherFlightRepository;
            _passengerRepository = passengerRepository;
            _timeProvider = timeProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// Searches for flights based on the provided search criteria.
        /// </summary>
        /// <param name="data">The search criteria for the flight search.</param>
        /// <returns>A list of flights that match the search criteria.</returns>
        [HttpPost("search-flight")]
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

            if (!string.IsNullOrEmpty(model.FlightNumber) && string.IsNullOrEmpty(model.AirlineId))
            {
                return BadRequest(new ApiResponse(400, "AirlineId must be specified when searching by flight number."));
            }

            if (!model.DepartureDate.HasValue && string.IsNullOrEmpty(model.AirlineId) &&
                string.IsNullOrEmpty(model.DestinationFrom) && string.IsNullOrEmpty(model.DestinationTo))
            {
                return BadRequest(new ApiResponse(400,
                    "At least one field must be filled in for the search criteria."));
            }

            Expression<Func<Flight, bool>> criteria = c =>
                (!model.DepartureDate.HasValue || c.DepartureDateTime.Date == model.DepartureDate.Value.Date) &&
                (string.IsNullOrEmpty(model.AirlineId) || c.AirlineId == model.AirlineId) &&
                (string.IsNullOrEmpty(model.DestinationFrom) || c.DestinationFromId == model.DestinationFrom) &&
                (string.IsNullOrEmpty(model.DestinationTo) || c.DestinationToId == model.DestinationTo) &&
                (string.IsNullOrEmpty(model.FlightNumber) || c.ScheduledFlightId.Substring(2) == model.FlightNumber);

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
        [HttpGet("flight/{id:guid}/details")]
        public async Task<ActionResult<Flight>> GetFlightDetails(Guid id)
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
        [HttpGet("flight/{id:guid}/onward-flights")]
        public async Task<ActionResult<List<BaseFlight>>> GetOnwardFlights(Guid id)
        {
            return await _GetConnectedFlights(id, bf =>
                bf.IteratedFlight.DepartureDateTime > bf.CurrentFlight.DepartureDateTime);
        }

        /// <summary>
        /// Retrieves a list of inbound flights related to a specific flight.
        /// </summary>
        /// <param name="id">The ID of the flight to retrieve inbound flights for.</param>
        /// <returns>A list of <see cref="BaseFlight"/> objects representing the inbound flights.</returns>
        [HttpGet("flight/{id:guid}/inbound-flights")]
        public async Task<ActionResult<List<BaseFlight>>> GetInboundFlights(Guid id)
        {
            return await _GetConnectedFlights(id, bf =>
                bf.IteratedFlight.DepartureDateTime < bf.CurrentFlight.DepartureDateTime);
        }

        private async Task<ActionResult<List<BaseFlight>>> _GetConnectedFlights(Guid id,
            Func<(Guid FlightId, BaseFlight IteratedFlight, BaseFlight CurrentFlight), bool> condition)
        {
            var currentFlight = await _flightRepository.GetFlightByIdAsync(id, false);

            if (currentFlight == null)
            {
                return NotFound(new ApiResponse(404, $"Flight {id} not found"));
            }

            var connectedFlights = new List<BaseFlight>();

            foreach (var passengerFlight in currentFlight.ListOfBookedPassengers)
            {
                var passenger = await _passengerRepository.GetPassengerByIdAsync(passengerFlight.PassengerOrItemId);

                var nonCurrentFlightsId = passenger.Flights
                    .Where(pf => pf.FlightId != currentFlight.Id && condition((pf.FlightId, pf.Flight, currentFlight)))
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

            var flightCounts = connectedFlights.GroupBy(f => f.Id)
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
        [HttpGet("flight/{id:guid}/passengers-with-onward-flight")]
        public async Task<ActionResult<List<Passenger>>> GetPassengersWithOnwardFlight(Guid id)
        {
            return await _GetPassengersWithFlightConnection(id, true);
        }

        /// <summary>
        /// Retrieves a list of passengers with an inbound flight for the specified flight ID.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>A list of passengers with an inbound flight.</returns>
        [HttpGet("flight/{id:guid}/passengers-with-inbound-flight")]
        public async Task<ActionResult<List<Passenger>>> GetPassengersWithInboundFlight(Guid id)
        {
            return await _GetPassengersWithFlightConnection(id, false);
        }

        private async Task<ActionResult<List<Passenger>>> _GetPassengersWithFlightConnection(Guid id,
            bool isOnwardFlight)
        {
            var passengers = await _passengerRepository.GetPassengersWithFlightConnectionsAsync(id, isOnwardFlight);

            var passengerDtos = _mapper.Map<List<PassengerDetailsDto>>(passengers, opt =>
                {
                    opt.Items["DepartureDateTime"] = passengers.FirstOrDefault()
                        ?.Flights.FirstOrDefault(pf => pf.FlightId == id)
                        ?.Flight.DepartureDateTime;
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
        
        /// <summary>
        /// Adds a connecting flight to a passenger.
        /// </summary>
        /// <param name="passengerId">The ID of the passenger.</param>
        /// <param name="id">The ID of the flight.</param>
        /// <param name="isInbound">Specifies if the connecting flight is inbound.</param>
        /// <param name="addConnectingFlightModels">The list of connecting flights to add.</param>
        /// <returns>An ActionResult of type BaseFlight.</returns>
        [HttpPost("flight/{id:guid}/passenger/{passengerId:guid}/add-connecting-flight")]
        public async Task<ActionResult<BaseFlight>> AddConnectingFlight(Guid id, Guid passengerId, bool isInbound,
            [FromBody] List<AddConnectingFlightModel> addConnectingFlightModels)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(passengerId);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {passengerId} was not found."));
            }

            var currentPassengerFlights = passenger.Flights.Select(pf => pf.Flight).ToList();
            var currentFlight = await _flightRepository.GetFlightByIdAsync(id, false);

            foreach (var connectingFlightModel in addConnectingFlightModels)
            {
                var parsedDepartureDateTime = _timeProvider.ParseDate(connectingFlightModel.DepartureDate).Value;

                var lastFlight = currentPassengerFlights[^1];

                var connectingFlight = await _GetOrCreateFlightAsync(connectingFlightModel, parsedDepartureDateTime);

                switch (isInbound)
                {
                    case false when
                        (connectingFlight as Flight)?.DepartureDateTime < (lastFlight as Flight)?.DepartureDateTime ||
                        connectingFlight.DepartureDateTime.Date < lastFlight.DepartureDateTime.Date ||
                        connectingFlight.DepartureDateTime > lastFlight.DepartureDateTime.AddDays(2):
                    {
                        return BadRequest(new ApiResponse(400,
                            "Connecting flight must be within 24 hours from last arrival."));
                    }
                    case true when ((connectingFlight as Flight)?.ArrivalDateTime > currentFlight.DepartureDateTime ||
                                    connectingFlight.DepartureDateTime.Date > currentFlight.DepartureDateTime.Date ||
                                    connectingFlight.DepartureDateTime < currentFlight.DepartureDateTime.AddDays(-2)):
                    {
                        return BadRequest(new ApiResponse(400,
                            "Inbound flight cannot be earlier than 24 hours before next departure"));
                    }
                }

                if (currentPassengerFlights.Contains(connectingFlight))
                {
                    return BadRequest(new ApiResponse(400, "Flight is already in passenger's itinerary."));
                }

                currentPassengerFlights.Add(connectingFlight);

                var newPassengerFlight = new PassengerFlight(passengerId, currentPassengerFlights.Last().Id,
                    connectingFlightModel.FlightClass);

                passenger.Flights.Add(newPassengerFlight);
            }

            await _passengerRepository.UpdateAsync(passenger);

            return Ok();
        }

        private async Task<BaseFlight> _GetOrCreateFlightAsync(AddConnectingFlightModel connectingFlightModel,
            DateTime parsedDepartureDateTime)
        {
            var flightCriteria = _BuildFlightCriteria(connectingFlightModel, parsedDepartureDateTime);
            var otherFlightCriteria = _BuildOtherFlightCriteria(connectingFlightModel, parsedDepartureDateTime);

            var connectingFlight = await _flightRepository.GetFlightByCriteriaAsync(flightCriteria, true);
            if (connectingFlight != null) return connectingFlight;

            var otherFlight = await _otherFlightRepository.GetOtherFlightByCriteriaAsync(otherFlightCriteria, true);
            if (otherFlight != null) return otherFlight;

            otherFlight = new OtherFlight(connectingFlightModel.FlightNumber, parsedDepartureDateTime, null,
                connectingFlightModel.DestinationFrom, connectingFlightModel.DestinationTo,
                connectingFlightModel.AirlineId);

            await _flightRepository.AddAsync(otherFlight);
            return otherFlight;
        }

        private static Expression<Func<Flight, bool>> _BuildFlightCriteria(
            AddConnectingFlightModel connectingFlightModel, DateTime parsedDepartureDateTime)
        {
            return f => f.AirlineId == connectingFlightModel.AirlineId &&
                        f.ScheduledFlightId.Substring(2) == connectingFlightModel.FlightNumber &&
                        f.DepartureDateTime.Date == parsedDepartureDateTime.Date &&
                        f.DestinationFromId == connectingFlightModel.DestinationFrom &&
                        f.DestinationToId == connectingFlightModel.DestinationTo;
        }

        private static Expression<Func<OtherFlight, bool>> _BuildOtherFlightCriteria(
            AddConnectingFlightModel connectingFlightModel, DateTime parsedDepartureDateTime)
        {
            return of => of.AirlineId == connectingFlightModel.AirlineId &&
                         of.FlightNumber == connectingFlightModel.FlightNumber &&
                         of.DepartureDateTime.Date == parsedDepartureDateTime.Date &&
                         of.DestinationFromId == connectingFlightModel.DestinationFrom && 
                         of.DestinationToId == connectingFlightModel.DestinationTo;
        }

        /// <summary>
        /// Deletes the connecting flight for a passenger.
        /// </summary>
        /// <param name="id">The id of the current flight.</param>
        /// <param name="passengerId">The id of the passenger.</param>
        /// <param name="flightIds">The list of flight ids to delete.</param>
        /// <returns>Returns an ActionResult of type BaseFlight.</returns>
        [HttpDelete("flight/{id:guid}/passenger/{passengerId:guid}/delete-connecting-flight")]
        public async Task<ActionResult<BaseFlight>> DeleteConnectingFlight(Guid id, Guid passengerId,
            [FromBody] List<Guid> flightIds)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(passengerId);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {passengerId} was not found."));
            }

            var flightsToDelete =
                await _baseFlightRepository.GetFlightsByCriteriaAsync(f => flightIds.Contains(f.Id) && f.Id != id,
                    true);

            if (flightsToDelete.Count == 0)
            {
                return BadRequest(new ApiResponse(400, "Invalid flight IDs."));
            }

            passenger.Flights.RemoveAll(pf => flightsToDelete.Contains(pf.Flight));
            await _passengerRepository.UpdateAsync(passenger);

            foreach (var flight in flightsToDelete)
            {
                if (flight.ListOfBookedPassengers.Count == 0)
                {
                    await _baseFlightRepository.DeleteAsync(flight);
                }
            }

            return NoContent();
        }
    }
}
