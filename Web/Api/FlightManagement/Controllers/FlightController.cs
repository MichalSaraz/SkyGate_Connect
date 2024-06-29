using System.Linq.Expressions;
using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.FlightContext.FlightInfo.Enums;
using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.Booking.Enums;
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
        private readonly ICommentRepository _commentRepository;
        private readonly ISpecialServiceRequestRepository _specialServiceRequestRepository;
        private readonly IBasePassengerOrItemRepository _basePassengerOrItemRepository;
        private readonly IInfantRepository _infantRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly IMapper _mapper;

        public FlightController(
            IFlightRepository flightRepository,
            IBaseFlightRepository baseFlightRepository,
            IOtherFlightRepository otherFlightRepository,
            IPassengerRepository passengerRepository,
            ICommentRepository commentRepository,
            ISpecialServiceRequestRepository specialServiceRequestRepository,
            IBasePassengerOrItemRepository basePassengerOrItemRepository,
            IInfantRepository infantRepository,
            ITimeProvider timeProvider,
            IMapper mapper)
        {
            _flightRepository = flightRepository;
            _baseFlightRepository = baseFlightRepository;
            _otherFlightRepository = otherFlightRepository;
            _passengerRepository = passengerRepository;
            _commentRepository = commentRepository;
            _specialServiceRequestRepository = specialServiceRequestRepository;
            _basePassengerOrItemRepository = basePassengerOrItemRepository;
            _infantRepository = infantRepository;
            _timeProvider = timeProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// Searches for flights based on the provided search criteria.
        /// </summary>
        /// <param name="data">The search criteria for the flight search.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /search-flights
        ///     {
        ///         "flightNumber": "1305",
        ///         "airlineId": "DY",
        ///         "departureDate": "18OCT",
        ///         "destinationFrom": "OSL",
        ///         "destinationTo": "LHR"
        ///     }
        ///
        /// </remarks> 
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="FlightDetailsDto"/> objects that match the search criteria.</returns>
        [HttpPost("search-flights")]
        public async Task<ActionResult<List<FlightDetailsDto>>> SearchFlights([FromBody] JObject data)
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

            if (model.DepartureDate.HasValue && string.IsNullOrEmpty(model.DestinationFrom) && 
                string.IsNullOrEmpty(model.DestinationTo) && string.IsNullOrEmpty(model.FlightNumber) &&
                string.IsNullOrEmpty(model.AirlineId))
            {
                return BadRequest(new ApiResponse(400,
                    "When DepartureDate is filled, at least DestinationTo or DestinationFrom should be filled."));
            }

            if (!string.IsNullOrEmpty(model.AirlineId) && (string.IsNullOrEmpty(model.DestinationTo) &&
                                                           string.IsNullOrEmpty(model.DestinationFrom)))
            {
                return BadRequest(new ApiResponse(400,
                    "When AirlineId is filled, at least DestinationTo or DestinationFrom should be filled."));
            }

            Expression<Func<Flight, bool>> criteria = c =>
                (!model.DepartureDate.HasValue || c.DepartureDateTime.Date == model.DepartureDate.Value.Date) &&
                (string.IsNullOrEmpty(model.AirlineId) || c.AirlineId == model.AirlineId) &&
                (string.IsNullOrEmpty(model.DestinationFrom) || c.DestinationFromId == model.DestinationFrom) &&
                (string.IsNullOrEmpty(model.DestinationTo) || c.DestinationToId == model.DestinationTo) &&
                (string.IsNullOrEmpty(model.FlightNumber) || c.ScheduledFlightId.Substring(2) == model.FlightNumber);

            var flights = await _flightRepository.GetFlightsByCriteriaAsync(criteria);

            if (flights.Count == 0)
            {
                return Ok(new ApiResponse(200, "No results found matching the specified criteria."));
            }
            
            var flightDtos = _mapper.Map<List<FlightDetailsDto>>(flights);

            return Ok(flightDtos);
        }


        /// <summary>
        /// Retrieves the details of a flight by its ID.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the <see cref="FlightDetailsDto"/> object that
        /// represents the flight details.</returns>
        [HttpGet("flight/{id:guid}/details")]
        public async Task<ActionResult<FlightDetailsDto>> GetFlightDetails(Guid id)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(id, false, true);

            if (flight is null or OtherFlight)
            {
                var message = flight == null
                    ? $"Flight {id} not found"
                    : $"It's not possible to get details for flight {id}";

                return NotFound(new ApiResponse(404, message));
            }

            var flightDto = _mapper.Map<FlightDetailsDto>(flight);

            return Ok(flightDto);
        }

        /// <summary>
        /// Get the onward flights for a specific flight.
        /// </summary>
        /// <param name="id">The ID of the flight to retrieve onward flights for.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="FlightConnectionsDto"/> objects.</returns>
        [HttpGet("flight/{id:guid}/onward-flights")]
        public async Task<ActionResult<List<FlightConnectionsDto>>> GetOnwardFlights(Guid id)
        {
            return await _GetConnectedFlights(id, bf =>
                bf.IteratedFlight.DepartureDateTime > bf.CurrentFlight.DepartureDateTime);
        }

        /// <summary>
        /// Retrieves a list of inbound flights related to a specific flight.
        /// </summary>
        /// <param name="id">The ID of the flight to retrieve inbound flights for.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="FlightConnectionsDto"/> objects.</returns>
        [HttpGet("flight/{id:guid}/inbound-flights")]
        public async Task<ActionResult<List<FlightConnectionsDto>>> GetInboundFlights(Guid id)
        {
            return await _GetConnectedFlights(id, bf =>
                bf.IteratedFlight.DepartureDateTime < bf.CurrentFlight.DepartureDateTime);
        }

        /// <summary>
        /// Retrieves a list of connected flights based on a given flight ID and condition.
        /// </summary>
        /// <param name="id">The ID of the current flight.</param>
        /// <param name="condition">A function that defines the condition for determining connected flights.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="FlightConnectionsDto"/> objects.</returns>
        private async Task<ActionResult<List<FlightConnectionsDto>>> _GetConnectedFlights(Guid id,
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

                if (passenger == null)
                {
                    return NotFound(new ApiResponse(404, $"Passenger {passengerFlight.PassengerOrItemId} not found"));
                }
                
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

            return flightDtos.Count == 0
                ? Ok(new ApiResponse(200, $"No connected flights found for this flight"))
                : Ok(flightDtos);
        }

        /// <summary>
        /// Gets the list of passengers with onward flight for a given flight ID.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="IEnumerable{T}"/> of objects representing
        /// passengers with an onward flight.</returns>
        [HttpGet("flight/{id:guid}/passengers-with-onward-flight")]
        public async Task<ActionResult<IEnumerable<object>>> GetPassengersWithOnwardFlight(Guid id)
        {
            return await _GetPassengersWithFlightConnection(id, true);
        }

        /// <summary>
        /// Retrieves a list of passengers with an inbound flight for the specified flight ID.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="IEnumerable{T}"/> of objects representing
        /// passengers with an inbound flight.</returns>
        [HttpGet("flight/{id:guid}/passengers-with-inbound-flight")]
        public async Task<ActionResult<IEnumerable<object>>> GetPassengersWithInboundFlight(Guid id)
        {
            return await _GetPassengersWithFlightConnection(id, false);
        }

        /// <summary>
        /// Get the passengers with flight connections for a specific flight.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <param name="isOnwardFlight">Flag indicating if the passengers should have onward flight connections or
        /// inbound flight connections.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="IEnumerable{T}"/> of objects.</returns>
        private async Task<ActionResult<IEnumerable<object>>> _GetPassengersWithFlightConnection(Guid id,
            bool isOnwardFlight)
        {
            var passengers = await _passengerRepository.GetPassengersWithFlightConnectionsAsync(id, isOnwardFlight);
            var flight = await _flightRepository.GetFlightByIdAsync(id, false);
            
            if (passengers.Count == 0)
            {
                return Ok(new ApiResponse(200, "No passengers with flight connection found on this flight"));
            }
            
            if (flight == null)
            {
                return NotFound(new ApiResponse(404, $"Flight {id} not found"));
            }
            
            var passengerDtos = _mapper.Map<List<PassengerDetailsDto>>(passengers, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = id;
                })
                .Select(pdd => new
                {
                    pdd.FirstName,
                    pdd.LastName,
                    pdd.Gender,
                    pdd.NumberOfCheckedBags,
                    pdd.SeatOnCurrentFlightDetails?.SeatNumber,
                    pdd.SeatOnCurrentFlightDetails?.FlightClass,
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
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="PassengerDetailsDto"/> object with added
        /// flights(s) included.</returns>
        [HttpPost("flight/{id:guid}/passenger/{passengerId:guid}/add-connecting-flight")]
        public async Task<ActionResult<PassengerDetailsDto>> AddConnectingFlight(Guid id, Guid passengerId,
            bool isInbound, [FromBody] List<AddConnectingFlightModel> addConnectingFlightModels)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(passengerId, true, true);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {passengerId} was not found."));
            }

            var currentPassengerFlights = passenger.Flights.Select(pf => pf.Flight).ToList();
            var currentFlight = await _flightRepository.GetFlightByIdAsync(id, false);
            
            if (currentFlight == null)
            {
                return NotFound(new ApiResponse(404, $"Flight {id} not found"));
            }

            foreach (var connectingFlightModel in addConnectingFlightModels)
            {
                var parsedDepartureDateTime = _timeProvider.ParseDate(connectingFlightModel.DepartureDate,
                    connectingFlightModel.DepartureTime);

                if (!parsedDepartureDateTime.HasValue)
                {
                    return BadRequest(new ApiResponse(400, "Invalid departure date."));
                }

                var lastFlight = currentPassengerFlights[^1];

                var connectingFlight =
                    await _GetOrCreateFlightAsync(connectingFlightModel, parsedDepartureDateTime.Value);

                switch (isInbound)
                {
                    case false when
                        connectingFlight.DepartureDateTime < lastFlight.DepartureDateTime ||
                        connectingFlight.DepartureDateTime > lastFlight.DepartureDateTime.AddDays(2):
                    {
                        return BadRequest(new ApiResponse(400,
                            "Connecting flight must be within 24 hours from last arrival."));
                    }
                    case true when
                        connectingFlight.DepartureDateTime > currentFlight.DepartureDateTime ||
                        connectingFlight.DepartureDateTime < currentFlight.DepartureDateTime.AddDays(-2):
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

            var passengerDto = _mapper.Map<PassengerDetailsDto>(passenger, opt =>
            {
                opt.Items["FlightId"] = id;
                opt.Items["DepartureDateTime"] = currentFlight.DepartureDateTime;
            });

            return Ok(passengerDto);
        }

        /// <summary>
        /// Retrieves an existing flight or creates a new flight based on the provided criteria.
        /// </summary>
        /// <param name="connectingFlightModel">The model containing the details of the connecting flight.</param>
        /// <param name="parsedDepartureDateTime">The parsed departure datetime of the connecting flight.</param>
        /// <returns>Returns a <see cref="Task{T}"/> representing the asynchronous operation that returns a
        /// <see cref="BaseFlight"/> object.</returns>
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

        /// <summary>
        /// Builds the flight criteria based on the given connecting flight model and parsed departure datetime.
        /// </summary>
        /// <param name="connectingFlightModel">The connecting flight model.</param>
        /// <param name="parsedDepartureDateTime">The parsed departure datetime.</param>
        /// <returns>The flight criteria as an expression.</returns>
        private static Expression<Func<Flight, bool>> _BuildFlightCriteria(
            AddConnectingFlightModel connectingFlightModel, DateTime parsedDepartureDateTime)
        {
            return f => f.AirlineId == connectingFlightModel.AirlineId &&
                        f.ScheduledFlightId.Substring(2) == connectingFlightModel.FlightNumber &&
                        f.DepartureDateTime == parsedDepartureDateTime &&
                        f.DestinationFromId == connectingFlightModel.DestinationFrom &&
                        f.DestinationToId == connectingFlightModel.DestinationTo;
        }

        /// <summary>
        /// Build the criteria for finding other flights based on the connecting flight model and the parsed departure
        /// datetime.
        /// </summary>
        /// <param name="connectingFlightModel">The connecting flight model</param>
        /// <param name="parsedDepartureDateTime">The parsed departure date time</param>
        /// <returns>The expression representing the criteria for finding other flights</returns>
        private static Expression<Func<OtherFlight, bool>> _BuildOtherFlightCriteria(
            AddConnectingFlightModel connectingFlightModel, DateTime parsedDepartureDateTime)
        {
            return of => of.AirlineId == connectingFlightModel.AirlineId &&
                         of.FlightNumber == connectingFlightModel.FlightNumber &&
                         of.DepartureDateTime == parsedDepartureDateTime &&
                         of.DestinationFromId == connectingFlightModel.DestinationFrom && 
                         of.DestinationToId == connectingFlightModel.DestinationTo;
        }

        /// <summary>
        /// Deletes a connecting flight for a passenger.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <param name="passengerId">The ID of the passenger.</param>
        /// <param name="flightIds">The list of flight IDs to delete.</param>
        /// <returns>A <see cref='NoContentResult'/> if the operation is successful.</returns>
        [HttpDelete("flight/{id:guid}/passenger/{passengerId:guid}/delete-connecting-flight")]
        public async Task<ActionResult> DeleteConnectingFlight(Guid id, Guid passengerId,
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

            if (flightsToDelete.Count < flightIds.Count)
            {
                return NotFound(new ApiResponse(404, "One or more flights were not found."));
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

        /// <summary>
        /// Retrieves the list of passengers along with their comments for a specific flight.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <param name="commentType">The type of comments to retrieve.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="PassengerOrItemCommentsDto"/> objects, each containing the details of a passenger along with
        /// their comments.</returns>
        [HttpGet("flight/{id:guid}/get-passengers-with-comments")]
        public async Task<ActionResult<List<PassengerOrItemCommentsDto>>> GetPassengersWithComments(Guid id,
            CommentTypeEnum commentType)
        {
            if (!await _flightRepository.ExistsAsync(id))
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {id} not found"));
            }

            var comments = await _commentRepository.GetCommentsByCriteriaAsync(c =>
                c.LinkedToFlights.Any(f => f.FlightId == id) && c.CommentType == commentType);
            
            if (comments.Count == 0)
            {
                return NotFound(new ApiResponse(200, "No comments found for this flight"));
            }

            var commentsGroupedByPassenger = comments.GroupBy(c => c.Passenger).ToList();

            var passengerCommentsDtoList = new List<PassengerOrItemCommentsDto>();

            foreach (var group in commentsGroupedByPassenger)
            {
                var passengerCommentsDto = _mapper.Map<PassengerOrItemCommentsDto>(group.Key, opt =>
                {
                    opt.Items["FlightId"] = id;
                });
                passengerCommentsDto.Comments = _mapper.Map<List<CommentDto>>(group.ToList());

                passengerCommentsDtoList.Add(passengerCommentsDto);
            }

            return Ok(passengerCommentsDtoList);
        }

        /// <summary>
        /// Get the list of passengers with special requests for a given flight ID and collection of special service
        /// request codes.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <param name="ssrCodes">The collection of special service request codes.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="PassengerSpecialServiceRequestsDto"/> objects.</returns>
        private async Task<ActionResult<List<PassengerSpecialServiceRequestsDto>>> _GetPassengersWithSpecialRequests(
            Guid id, ICollection<string> ssrCodes)
        {
            if (!await _flightRepository.ExistsAsync(id))
            {
                return NotFound(new ApiResponse(404, $"Flight with {id} not found"));
            }

            var recordsWithSpecialRequests =
                await _specialServiceRequestRepository.GetSpecialServiceRequestsByCriteriaAsync(ssr => 
                ssr.FlightId == id && ssrCodes.Contains(ssr.SSRCode.Code));

            if (recordsWithSpecialRequests.Count == 0)
            {
                return Ok(new ApiResponse(200,
                    $"No passengers with service request {string.Join(", ", ssrCodes)} found for this flight"));
            }

            var ssrGroupedByPassenger = recordsWithSpecialRequests.GroupBy(ssr => ssr.Passenger).ToList();

            var passengerSpecialServiceRequestsDtoList = new List<PassengerSpecialServiceRequestsDto>();

            foreach (var group in ssrGroupedByPassenger)
            {
                var passengerSpecialServiceRequestsDto =
                    _mapper.Map<PassengerSpecialServiceRequestsDto>(group.Key, opt => { opt.Items["FlightId"] = id; });
                passengerSpecialServiceRequestsDto.SpecialServiceRequests =
                    _mapper.Map<List<SpecialServiceRequestDto>>(group.ToList());

                passengerSpecialServiceRequestsDtoList.Add(passengerSpecialServiceRequestsDto);
            }

            return Ok(passengerSpecialServiceRequestsDtoList);
        }

        /// <summary>
        /// Retrieves a list of passengers with special assistance for a specific flight.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="PassengerSpecialServiceRequestsDto"/> objects.</returns>
        [HttpGet("flight/{id:guid}/passengers-with-special-assistance")]
        public async Task<ActionResult<List<PassengerSpecialServiceRequestsDto>>> GetPassengersWithSpecialAssistance(
            Guid id)
        {
            var specialAssistanceSSRCodes = new List<string> { "WCHC", "WCHR", "WCHS", "MAAS" };
            return await _GetPassengersWithSpecialRequests(id, specialAssistanceSSRCodes);
        }

        /// <summary>
        /// Retrieves a list of passengers with disability for a specific flight.
        /// </summary>
        /// <param name="id">The unique identifier of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="PassengerSpecialServiceRequestsDto"/> objects.</returns>
        [HttpGet("flight/{id:guid}/passengers-with-disability")]
        public async Task<ActionResult<List<PassengerSpecialServiceRequestsDto>>> GetPassengersWithDisability(Guid id)
        {
            var disabilitySSRCodes = new List<string> { "MEDA", "STCR", "DPNA", "DEAF", "BLND" };
            return await _GetPassengersWithSpecialRequests(id, disabilitySSRCodes);
        }

        /// <summary>
        /// Retrieves the list of deportee passengers for a given flight.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="PassengerSpecialServiceRequestsDto"/> objects.</returns>
        [HttpGet("flight/{id:guid}/deportee-passengers")]
        public async Task<ActionResult<List<PassengerSpecialServiceRequestsDto>>> GetDeporteePassengers(Guid id)
        {
            var deporteeSSRCodes = new List<string> { "DEPA", "DEPU", "PICA", "PICU" };
            return await _GetPassengersWithSpecialRequests(id, deporteeSSRCodes);
        }

        /// <summary>
        /// Returns a list of passengers with animals on a flight.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="PassengerSpecialServiceRequestsDto"/> objects.</returns>
        [HttpGet("flight/{id:guid}/passengers-with-animals")]
        public async Task<ActionResult<List<PassengerSpecialServiceRequestsDto>>> GetPassengersWithAnimals(Guid id)
        {
            var animalsSSRCodes = new List<string> { "SVAN", "ESAN", "PETC", "AVIH" };
            return await _GetPassengersWithSpecialRequests(id, animalsSSRCodes);
        }

        /// <summary>
        /// Retrieves a list of unaccompanied minors for a specific flight.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="PassengerSpecialServiceRequestsDto"/> objects.</returns>
        [HttpGet("flight/{id:guid}/unaccompanied-minors")]
        public async Task<ActionResult<List<PassengerSpecialServiceRequestsDto>>> GetUnaccompaniedMinors(Guid id)
        {
            var unaccompaniedMinorSSRCodes = new List<string> { "UMNR" };
            return await _GetPassengersWithSpecialRequests(id, unaccompaniedMinorSSRCodes);
        }

        /// <summary>
        /// Get the list of passengers with sport equipment for a given flight.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="PassengerSpecialServiceRequestsDto"/> objects.</returns>
        [HttpGet("flight/{id:guid}/passengers-with-sport-equipment")]
        public async Task<ActionResult<List<PassengerSpecialServiceRequestsDto>>> GetPassengersWithSportEquipment(
            Guid id)
        {
            var sportEquipmentSSRCodes = new List<string> { "SPEQ", "BIKE" };
            return await _GetPassengersWithSpecialRequests(id, sportEquipmentSSRCodes);
        }

        /// <summary>
        /// Retrieves a list of passengers with a firearm on a specific flight.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="PassengerSpecialServiceRequestsDto"/> objects.</returns>
        [HttpGet("flight/{id:guid}/passengers-with-firearm")]
        public async Task<ActionResult<List<PassengerSpecialServiceRequestsDto>>> GetPassengersWithFirearm(Guid id)
        {
            var firearmSSRCodes = new List<string> { "WEAP" };
            return await _GetPassengersWithSpecialRequests(id, firearmSSRCodes);
        }

        /// <summary>
        /// Retrieves a list of infants for a specific flight.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="InfantOverviewDto"/> objects.</returns>
        [HttpGet("flight/{id:guid}/infants")]
        public async Task<ActionResult<List<InfantOverviewDto>>> GetInfants(Guid id)
        {
            if (await _flightRepository.GetFlightByIdAsync(id) is not Flight flight)
            {
                return NotFound(new ApiResponse(404, $"Flight {id} not found"));
            }

            var infants = await _infantRepository.GetInfantsByCriteriaAsync(p =>
                p.Flights.Any(f => f.FlightId == id));
            
            if (infants.Count == 0)
            {
                return Ok(new ApiResponse(200, "No infants found for this flight"));
            }

            var infantList = _mapper.Map<List<InfantOverviewDto>>(infants, opt =>
            {
                opt.Items["FlightId"] = id;
                opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
            });

            return Ok(infantList);
        }

        /// <summary>
        /// Retrieves a list of either CabinBaggageRequiringSeat or ExtraSeat associated with a specific flight.
        /// </summary>
        /// <param name="id">The unique identifier of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list of <see cref="List{T}"/> of
        /// <see cref="ItemOverviewDto"/> objects.</returns>
        [HttpGet("flight/{id:guid}/passengers-with-cbbg-or-exst")]
        public async Task<ActionResult<List<ItemOverviewDto>>> GetCabinBaggageRequiringSeatOrExtraSeat(Guid id)
        {
            if (await _flightRepository.GetFlightByIdAsync(id) is not Flight flight)
            {
                return NotFound(new ApiResponse(404, $"Flight {id} not found"));
            }

            var cabinBaggageRequiringSeatOrExtraSeats =
                await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(
                p => p.Flights.Any(f => f.FlightId == id) && (p is CabinBaggageRequiringSeat || p is ExtraSeat));
            
            if (cabinBaggageRequiringSeatOrExtraSeats.Count == 0)
            {
                return Ok(new ApiResponse(200, "No Cabin baggage requiring seat or Extra seat found for this flight"));
            }

            var cabinBaggageRequiringSeatOrExtraSeatList = _mapper.Map<List<ItemOverviewDto>>(
                cabinBaggageRequiringSeatOrExtraSeats, opt =>
                {
                    opt.Items["FlightId"] = id;
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                });

            return Ok(cabinBaggageRequiringSeatOrExtraSeatList);
        }

        /// <summary>
        /// Retrieves a list of passengers for a given flight.
        /// </summary>
        /// <param name="id">The ID of the flight.</param>
        /// <param name="acceptanceStatus">The acceptance status of the passengers.</param>
        /// <param name="applyAcceptanceStatusFilter">A flag indicating whether to apply the acceptance status
        /// filter.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="BasePassengerOrItemDto"/> objects.</returns>
        [HttpGet("flight/{id:guid}/passenger-list")]
        public async Task<ActionResult<List<BasePassengerOrItemDto>>> GetPassengerList(Guid id,
            AcceptanceStatusEnum acceptanceStatus, bool applyAcceptanceStatusFilter)
        {
            if (await _flightRepository.GetFlightByIdAsync(id) is not Flight flight)
            {
                return NotFound(new ApiResponse(404, $"Flight {id} not found"));
            }

            Expression<Func<BasePassengerOrItem, bool>> criteria = p => p.Flights.Any(f => f.FlightId == id);

            if (applyAcceptanceStatusFilter)
            {
                criteria = p => p.Flights.Any(f => f.FlightId == id && f.AcceptanceStatus == acceptanceStatus);
            }

            var passengerOrItems =
                await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(criteria);

            if (passengerOrItems.Count == 0)
            {
                return Ok(new ApiResponse(200, $"No {acceptanceStatus} passengers or items found for this flight"));
            }

            var passengerOrItemDtos = _mapper.Map<List<BasePassengerOrItemDto>>(passengerOrItems, opt =>
            {
                opt.Items["FlightId"] = id;
                opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
            });

            return Ok(passengerOrItemDtos);
        }

        /// <summary>
        /// Updates the flight status of a specific flight.
        /// </summary>
        /// <param name="id">The unique identifier of the flight.</param>
        /// <param name="flightStatus">The new flight status.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the updated <see cref="FlightDetailsDto"/> object.
        /// </returns>
        [HttpPatch("flight/{id:guid}/update-flight-status")]
        public async Task<ActionResult<FlightDetailsDto>> UpdateFlightStatus(Guid id, FlightStatusEnum flightStatus)
        {
            if (await _flightRepository.GetFlightByIdAsync(id) is not Flight flight)
            {
                return NotFound(new ApiResponse(404, $"Flight {id} not found"));
            }

            flight.FlightStatus = flightStatus;

            await _flightRepository.UpdateAsync(flight);

            var updatedFlightDto = _mapper.Map<FlightDetailsDto>(flight);

            return Ok(updatedFlightDto);
        }
    }
}
