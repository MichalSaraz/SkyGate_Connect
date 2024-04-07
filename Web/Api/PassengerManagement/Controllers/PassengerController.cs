using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.Internal;
using Core.BaggageContext;
using Core.Dtos;
using Core.FlightContext;
using Core.Interfaces;
using Core.PassengerContext;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Web.Api.PassengerManagement.Models;
using Web.Errors;

namespace Web.Api.PassengerManagement.Controllers
{
    [ApiController]
    [Route("passenger-management")]
    public class PassengerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITimeProvider _timeProvider;
        private readonly IFlightRepository _flightRepository;
        private readonly IPassengerRepository _passengerRepository;

        public PassengerController(
            IMapper mapper,
            ITimeProvider timeProvider,
            IFlightRepository flightRepository,
            IPassengerRepository passengerRepository)
        {
            _mapper = mapper;
            _timeProvider = timeProvider;
            _flightRepository = flightRepository;
            _passengerRepository = passengerRepository;
        }

        /// <summary>
        /// Searches for passengers based on the given search criteria.
        /// </summary>
        /// <param name="data">The search criteria as a JObject.</param>
        /// <returns>Returns a list of Passenger objects that match the search criteria.</returns>
        [HttpPost("search-passenger")]
        public async Task<ActionResult<List<Passenger>>> SearchPassengers([FromBody] JObject data)
        {
            var model = new PassengerSearchModel
            {
                //ToDo : ToUpper(), ToLower() apply
                FlightNumber = data["flightNumber"]?.ToString(),
                AirlineId = data["airlineId"]?.ToString(),
                DepartureDate = _timeProvider.ParseDate(data["departureDate"]?.ToString()),
                DocumentNumber = data["documentNumber"]?.ToString(),
                LastName = data["lastName"]?.ToString(),
                PNR = data["pnr"]?.ToString(),
                DestinationFrom = data["destinationFrom"]?.ToString(),
                DestinationTo = data["destinationTo"]?.ToString(),
                SeatNumber = data["seatNumber"]?.ToString()
            };

            if (string.IsNullOrEmpty(model.FlightNumber) || string.IsNullOrEmpty(model.AirlineId) ||
                !model.DepartureDate.HasValue || (string.IsNullOrEmpty(model.DocumentNumber) &&
                                                  string.IsNullOrEmpty(model.LastName) &&
                                                  string.IsNullOrEmpty(model.PNR) &&
                                                  string.IsNullOrEmpty(model.DestinationFrom) &&
                                                  string.IsNullOrEmpty(model.DestinationTo) &&
                                                  string.IsNullOrEmpty(model.SeatNumber)))
            {
                return BadRequest(new ApiResponse(400,
                    "All mandatory plus one optional field must be filled in for the search criteria."));
            }

            Expression<Func<Passenger, bool>> criteria = c =>
                c.Flights.Any(pf =>
                    pf.Flight is Flight && (pf.Flight as Flight).ScheduledFlightId.Substring(2) == model.FlightNumber &&
                    pf.Flight.AirlineId == model.AirlineId &&
                    pf.Flight.DepartureDateTime.Date == model.DepartureDate.Value.Date) &&
                (string.IsNullOrEmpty(model.DocumentNumber) ||
                 c.TravelDocuments.Any(a => a.DocumentNumber == model.DocumentNumber)) &&
                (string.IsNullOrEmpty(model.DestinationFrom) ||
                 c.Flights.Any(bf => bf.Flight.DestinationFromId == model.DestinationFrom)) &&
                (string.IsNullOrEmpty(model.DestinationTo) ||
                 c.Flights.Any(bf => bf.Flight.DestinationToId == model.DestinationTo)) &&
                (string.IsNullOrEmpty(model.SeatNumber) ||
                 c.AssignedSeats.Any(s => s.SeatNumber == model.SeatNumber)) &&
                (string.IsNullOrEmpty(model.LastName) ||
                 c.LastName.Substring(0, model.LastName.Length) == model.LastName) &&
                (string.IsNullOrEmpty(model.PNR) || c.BookingDetails.PNRId == model.PNR);

            var passengers = await _passengerRepository.GetPassengersByCriteriaAsync(criteria);

            var selectedFlight = await _flightRepository.GetFlightByCriteriaAsync(f =>
                f.ScheduledFlightId.Substring(2) == model.FlightNumber && f.AirlineId == model.AirlineId &&
                f.DepartureDateTime.Date == model.DepartureDate.Value.Date);

            var passengersDto = _mapper.Map<List<PassengerOverviewDto>>(passengers, opt =>
            {
                opt.Items["DepartureDateTime"] = selectedFlight.DepartureDateTime;
                opt.Items["FlightId"] = selectedFlight.Id;
            });

            return Ok(passengersDto);
        }

        /// <summary>
        /// Get passenger details for a given flight and list of passenger ids.
        /// </summary>
        /// <param name="flightId">The id of the flight.</param>
        /// <param name="passengerIds">The list of passenger ids.</param>
        /// <returns>
        /// An ActionResult containing the list of passenger details as PassengerDetailsDto objects.
        /// If no passengers are found for the provided ids, returns a NotFound response with an ApiResponse message.
        /// </returns>
        [HttpPost("selected-flight/{flightId:guid}/passenger-details")]
        public async Task<ActionResult<Passenger>> GetPassengerDetails(Guid flightId,
            [FromBody] List<Guid> passengerIds)
        {
            Expression<Func<Passenger, bool>> criteria = c => passengerIds.Contains(c.Id);

            var passengers = await _passengerRepository.GetPassengersByCriteriaAsync(criteria, false, true);
            var selectedFlight = await _flightRepository.GetFlightByIdAsync(flightId, false);

            var passengersDto = passengers.Select(passenger => _mapper.Map<PassengerDetailsDto>(passenger, opt =>
            {
                opt.Items["DepartureDateTime"] = selectedFlight.DepartureDateTime;
                opt.Items["FlightId"] = flightId;
            }));

            if (!passengersDto.Any())
            {
                return NotFound(new ApiResponse(404, $"Passengers not found for provided ids"));
            }

            return Ok(passengersDto);
        }

        /// <summary>
        /// Retrieves a list of passengers by booking reference.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="bookingReference">The booking reference.</param>
        /// <returns>
        /// An async task that returns an <see cref="ActionResult"/> of <see cref="List{Passenger}"/>.
        /// If no passengers are found with the given booking reference, returns a <see cref="NotFoundObjectResult"/>
        /// with an <see cref="ApiResponse"/> object that has a status code of 404 and an error message indicating
        /// that no passengers were found.
        /// If passengers are found, returns an <see cref="OkObjectResult"/> with a collection of passengers.
        /// </returns>
        [HttpGet("selected-flight/{flightId:guid}/booking-reference/{bookingReference}/passenger-list")]
        public async Task<ActionResult<List<Passenger>>> GetPassengersByBookingReference(Guid flightId,
            string bookingReference)
        {
            Expression<Func<Passenger, bool>> criteria = c => c.BookingDetails.PNRId == bookingReference;

            var passengers = await _passengerRepository.GetPassengersByCriteriaAsync(criteria, false, true);
            var selectedFlight = await _flightRepository.GetFlightByIdAsync(flightId, false);

            var passengersDto = passengers.Select(passenger => _mapper.Map<PassengerOverviewDto>(passenger, opt =>
            {
                opt.Items["DepartureDateTime"] = selectedFlight.DepartureDateTime;
                opt.Items["FlightId"] = flightId;
            }));

            if (!passengersDto.Any())
            {
                return NotFound(new ApiResponse(404, $"No passengers found with Booking Reference {bookingReference}"));
            }

            return Ok(passengersDto);
        }

        /// <summary>
        /// Retrieves all the bags of a passenger based on the provided id.
        /// </summary>
        /// <param name="id">The id of the passenger.</param>
        /// <returns>A list of BaggageDetailsDto objects representing the bags of the passenger.</returns>
        [HttpGet("passenger/{id:guid}/all-bags")]
        public async Task<ActionResult<List<Baggage>>> GetAllPassengersBags(Guid id)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id, false, true);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            var passengerDto = _mapper.Map<List<BaggageDetailsDto>>(passenger.PassengerCheckedBags);

            return Ok(passengerDto);
        }

        /// <summary>
        /// Add passengers to the selection for a specific flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight to add passengers to.</param>
        /// <param name="passengerSelectionUpdate">The model containing passenger selection updates.</param>
        /// <returns>
        /// The list of passengers with additional details (PassengerDetailsDto) for the selected flight.
        /// </returns>
        [HttpPost("selected-flight/{flightId:guid}/add-passengers")]
        public async Task<ActionResult<List<Passenger>>> AddPassengersToSelection(Guid flightId,
            [FromBody] PassengerSelectionUpdateModel passengerSelectionUpdate)
        {
            var selectedFlight = await _flightRepository.GetFlightByIdAsync(flightId, false);

            if (selectedFlight == null)
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} was not found."));
            }

            var existingPassengers =
                await _passengerRepository.GetPassengersByCriteriaAsync(
                    p => passengerSelectionUpdate.ExistingPassengers.Contains(p.Id), true, true);

            var passengersToAdd = 
                await _passengerRepository.GetPassengersByCriteriaAsync(
                    p => passengerSelectionUpdate.PassengersToAdd.Contains(p.Id), true, true);
                
            
            if (!existingPassengers.Any() || !passengersToAdd.Any())
            {
                return NotFound(new ApiResponse(404, $"No passengers found with provided ids"));
            }

            existingPassengers.Concat(passengersToAdd);

            var passengersDto = existingPassengers.Select(passenger => _mapper.Map<PassengerDetailsDto>(
                passenger, opt =>
                {
                    opt.Items["DepartureDateTime"] = selectedFlight.DepartureDateTime;
                    opt.Items["FlightId"] = flightId;
                }));

            return Ok(passengersDto);
        }
    }
}