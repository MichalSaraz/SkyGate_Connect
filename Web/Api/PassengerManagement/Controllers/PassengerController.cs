using System.Linq.Expressions;
using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.FlightContext.FlightInfo.Enums;
using Core.HistoryTracking;
using Core.HistoryTracking.Enums;
using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext;
using Core.SeatingContext.Enums;
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
        private readonly IInfantRepository _infantRepository;
        private readonly IPassengerBookingDetailsRepository _passengerBookingDetailsRepository;
        private readonly IPassengerFlightRepository _passengerFlightRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IBasePassengerOrItemRepository _basePassengerOrItemRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IPassengerHistoryService _passengerHistoryService;
        private readonly IPassengerDtoMappingService _passengerDtoMappingService;

        public PassengerController(
            IMapper mapper,
            ITimeProvider timeProvider,
            IFlightRepository flightRepository,
            IPassengerRepository passengerRepository,
            IInfantRepository infantRepository,
            IPassengerBookingDetailsRepository passengerBookingDetailsRepository,
            IPassengerFlightRepository passengerFlightRepository,
            ISeatRepository seatRepository,
            IBasePassengerOrItemRepository basePassengerOrItemRepository, 
            IActionHistoryRepository actionHistoryRepository, 
            IPassengerHistoryService passengerHistoryService,
            IPassengerDtoMappingService passengerDtoMappingService)
        {
            _mapper = mapper;
            _timeProvider = timeProvider;
            _flightRepository = flightRepository;
            _passengerRepository = passengerRepository;
            _infantRepository = infantRepository;
            _passengerBookingDetailsRepository = passengerBookingDetailsRepository;
            _passengerFlightRepository = passengerFlightRepository;
            _seatRepository = seatRepository;
            _basePassengerOrItemRepository = basePassengerOrItemRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _passengerHistoryService = passengerHistoryService;
            _passengerDtoMappingService = passengerDtoMappingService;
        }

        /// <summary>
        /// Searches for passengers based on the given search criteria.
        /// </summary>
        /// <param name="data">The search criteria as a JObject.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /search-passengers
        ///     {
        ///         "flightNumber": "1305",
        ///         "airlineId": "DY",
        ///         "departureDate": "18OCT",
        ///         "destinationFrom": "OSL",
        ///         "destinationTo": "LHR",
        ///         "documentNumber": "12345789",
        ///         "lastName": "Doe",
        ///         "pnr": "ABC123",
        ///         "seatNumber": "1A"
        ///     }
        ///
        /// </remarks> 
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of <see cref="object"/>.
        /// </returns>
        [HttpPost("search-passengers")]
        public async Task<ActionResult<List<BasePassengerOrItemDto>>> SearchPassengers([FromBody] JObject data)
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
                    "Flight number, Airline, Departure date plus one optional field must be filled in for the search criteria."));
            }
            
            Expression<Func<BasePassengerOrItem, bool>> criteria = c =>
                c.Flights.Any(pf =>
                    pf.Flight is Flight && ((Flight)pf.Flight).ScheduledFlightId.Substring(2) == model.FlightNumber &&
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

            var passengerOrItems = 
                await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(criteria, false);

            var selectedFlight = await _flightRepository.GetFlightByCriteriaAsync(f =>
                f.ScheduledFlightId.Substring(2) == model.FlightNumber && f.AirlineId == model.AirlineId &&
                f.DepartureDateTime.Date == model.DepartureDate.Value.Date);
            
            if (!passengerOrItems.Any())
            {
                return Ok(new ApiResponse(200, "No passengers found with provided search criteria"));
            }
            
            if (selectedFlight == null)
            {
                return NotFound(new ApiResponse(404, "Flight not found with provided search criteria"));
            }

            var passengerOrItemsDto = _mapper.Map<List<BasePassengerOrItemDto>>(passengerOrItems, opt =>
            {
                opt.Items["FlightId"] = selectedFlight.Id;
            });
            
            return Ok(passengerOrItemsDto);
        }

        /// <summary>
        /// Get passenger details for a given flight and list of passenger ids.
        /// </summary>
        /// <param name="flightId">The id of the flight.</param>
        /// <param name="passengerIds">The list of passenger ids.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of <see cref="object"/>.
        /// </returns>
        [HttpPost("selected-flight/{flightId:guid}/passenger-details")]
        public async Task<ActionResult<List<object>>> GetPassengerDetails(Guid flightId,
            [FromBody] List<Guid> passengerIds)
        {
            if (await _flightRepository.GetFlightByIdAsync(flightId, false) is not Flight selectedFlight)
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} was not found."));
            }
            
            var passengerOrItems = await _CastPassengerOrItems(c => passengerIds.Contains(c.Id));
            
            if (!passengerOrItems.Any())
            {
                return NotFound(new ApiResponse(404, "Passengers not found for provided ids"));
            }
            
            var passengerOrItemsDto =
                _passengerDtoMappingService.MapPassengerOrItemsToDto(passengerOrItems, selectedFlight);

            return Ok(passengerOrItemsDto);
        }

        /// <summary>
        /// Casts Passenger, CabinBaggageRequiringSeat, ExtraSeat or Infant objects from a list of
        /// <see cref="BasePassengerOrItem"/> objects based on provided criteria.
        /// </summary>
        /// <param name="criteria">The expression representing the criteria to filter the
        /// <see cref="BasePassengerOrItem"/> objects.</param>
        /// <returns>A list <see cref="List{T}"/> of <see cref="CabinBaggageRequiringSeat"/>, <see cref="ExtraSeat"/>,
        /// <see cref="Passenger"/> or <see cref="Infant"/> objects.</returns>
        private async Task<List<object>> _CastPassengerOrItems(Expression<Func<BasePassengerOrItem, bool>> criteria)
        {
            var passengerOrItemsList = new List<object>();
            var passengerOrItems = 
                await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(
                    criteria, false);

            foreach (var passengerOrItem in passengerOrItems)
            {
                if (passengerOrItem is Passenger passenger)
                {
                    passenger = await _passengerRepository.GetPassengerByIdAsync(passenger.Id, false, true);
                    passengerOrItemsList.Add(passenger);
                }
                else if (passengerOrItem is CabinBaggageRequiringSeat cbbg)
                {
                    cbbg =
                        await _basePassengerOrItemRepository.GetBasePassengerOrItemByIdAsync(cbbg.Id) as
                            CabinBaggageRequiringSeat ?? cbbg;
                    passengerOrItemsList.Add(cbbg);
                }
                else if (passengerOrItem is ExtraSeat exst)
                {
                    exst = await _basePassengerOrItemRepository.GetBasePassengerOrItemByIdAsync(exst.Id) as ExtraSeat ??
                           exst;
                    passengerOrItemsList.Add(exst);
                }
                else if (passengerOrItem is Infant infant)
                {
                    infant = await _infantRepository.GetInfantByIdAsync(infant.Id);
                    passengerOrItemsList.Add(infant);
                }
            }
            
            return passengerOrItemsList;
        }

        /// <summary>
        /// Retrieves a list of passengers by booking reference for a specific flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="bookingReference">The booking reference.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of <see cref="object"/>.
        /// </returns>
        [HttpGet("selected-flight/{flightId:guid}/booking-reference/{bookingReference}/passenger-list")]
        public async Task<ActionResult<List<object>>> GetPassengersByBookingReference(Guid flightId,
            string bookingReference)
        {
            if (await _flightRepository.GetFlightByIdAsync(flightId, false) is not Flight selectedFlight)
            {
                return NotFound(new ApiResponse(404, "Flight not found with provided id"));
            }

            var passengerOrItems = await _CastPassengerOrItems(c => c.BookingDetails.PNRId == bookingReference);
            
            if (!passengerOrItems.Any())
            {
                return NotFound(new ApiResponse(404,
                    $"No passengers or items found with Booking Reference {bookingReference}"));
            }
            
            var passengerOrItemsDto = 
                _passengerDtoMappingService.MapPassengerOrItemsToDto(passengerOrItems, selectedFlight, false);

            return Ok(passengerOrItemsDto);
        }

        /// <summary>
        /// Retrieves all the bags of a passenger based on the provided id.
        /// </summary>
        /// <param name="flightId">The id of the flight.</param>
        /// <param name="id">The id of the passenger.</param>
        /// <returns>An <see cref="ActionResult{T}"/> of type <see cref="PassengerDetailsDto"/> containing the passenger
        /// bags.</returns>
        [HttpGet("selected-flight/{flightId:guid}/passenger/{id:guid}/all-bags")]
        public async Task<ActionResult<PassengerDetailsDto>> GetAllPassengersBags(Guid flightId, Guid id)
        {
            if (await _flightRepository.GetFlightByIdAsync(flightId, false) is not Flight selectedFlight)
            {
                return NotFound(new ApiResponse(404, "Flight not found with provided id"));
            }

            var passenger = await _passengerRepository.GetPassengerByIdAsync(id, false, true);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            var passengerDto = _mapper.Map<PassengerDetailsDto>(passenger, opt =>
            {
                opt.Items["DepartureDateTime"] = selectedFlight.DepartureDateTime;
                opt.Items["FlightId"] = flightId;
            });

            return Ok(passengerDto.PassengerCheckedBags);
        }

        /// <summary>
        /// Add passengers to the selection for a specific flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight to add passengers to.</param>
        /// <param name="passengerSelectionUpdate">The model containing passenger selection updates.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of <see cref="object"/>.
        /// </returns>
        [HttpPost("selected-flight/{flightId:guid}/add-passengers")]
        public async Task<ActionResult<List<object>>> AddPassengersToSelection(Guid flightId,
            [FromBody] PassengerSelectionUpdateModel passengerSelectionUpdate)
        {
            if (await _flightRepository.GetFlightByIdAsync(flightId, false) is not Flight selectedFlight)
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} was not found."));
            }
            
            var existingPassengerOrItems = await _CastPassengerOrItems(p =>
                passengerSelectionUpdate.ExistingPassengers.Contains(p.Id));
            
            var passengerOrItemsToAdd = await _CastPassengerOrItems(p =>
                passengerSelectionUpdate.PassengersToAdd.Contains(p.Id));

            if (!existingPassengerOrItems.Any() || !passengerOrItemsToAdd.Any())
            {
                return NotFound(new ApiResponse(404, $"No passengers or items found with provided ids"));
            }

            if (passengerOrItemsToAdd.Any(p => p is Infant && passengerOrItemsToAdd.All(p2 =>
                    (p2 as BasePassengerOrItem)?.Id != (p as Infant)?.AssociatedAdultPassengerId)))
            {
                return BadRequest(new ApiResponse(400, "Infant must be added with the associated adult passenger"));
            }

            var existingPassengerList = new List<object>(existingPassengerOrItems);
            existingPassengerList.AddRange(passengerOrItemsToAdd);

            var passengerOrItemsDto =
                _passengerDtoMappingService.MapPassengerOrItemsToDto(existingPassengerList, selectedFlight);

            return Ok(passengerOrItemsDto);
        }

        /// <summary>
        /// Adds an infant to a passenger.
        /// </summary>
        /// <param name="flightId"></param>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="infantModel">The details of the infant to be added.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="object"/> object.</returns>
        [HttpPost("selected-flight/{flightId:guid}/passenger/{id:guid}/add-infant")]
        public async Task<ActionResult<object>> AddInfant(Guid flightId, Guid id,
            [FromBody] InfantModel infantModel)
        {
            if (await _flightRepository.GetFlightByIdAsync(flightId, false) is not Flight selectedFlight)
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} was not found."));
            }

            var passenger = await _passengerRepository.GetPassengerByIdAsync(id, true, true);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            if (passenger.Infant != null)
            {
                return BadRequest(new ApiResponse(400, $"Passenger with Id {id} already has an infant associated."));
            }

            if (passenger.BookingDetails.Age < 18)
            {
                return BadRequest(new ApiResponse(400,
                    $"Passenger with Id {id} must be 18 years or older to have an infant associated."));
            }

            if (passenger.Flights.Any(f => f.Flight.DepartureDateTime >= selectedFlight.DepartureDateTime &&
                f.AcceptanceStatus == AcceptanceStatusEnum.Accepted))
            {
                return BadRequest(new ApiResponse(400,
                    "Infant cannot be added if the passenger is already checked in"));
            }

            var infant = new Infant(id, infantModel.FirstName, infantModel.LastName, infantModel.Gender,
                passenger.BookingDetails.AssociatedPassengerBookingDetailsId, 0);

            foreach (var flight in passenger.Flights)
            {
                infant.Flights.Add(new PassengerFlight(infant.Id, flight.FlightId, flight.FlightClass));

                if (passenger.SpecialServiceRequests.All(ssr => ssr.SSRCodeId != "INFT"))
                {
                    passenger.SpecialServiceRequests.Add(
                        new SpecialServiceRequest("INFT", flight.FlightId, infant.Id, infantModel.FreeText));
                }
            }
            
            await _infantRepository.AddAsync(infant);
            
            passenger.InfantId = infant.Id;

            Expression<Func<PassengerBookingDetails, bool>> criteria = c => c.Id == infant.BookingDetailsId;
            var bookingDetails = await _passengerBookingDetailsRepository.GetBookingDetailsByCriteriaAsync(criteria);

            if (bookingDetails != null)
            {
                bookingDetails.PassengerOrItemId = infant.Id;
                await _passengerBookingDetailsRepository.UpdateAsync(bookingDetails);
            }
            
            var updatedPassengerDto = 
                _passengerDtoMappingService.MapSinglePassengerOrItemToDto(passenger, selectedFlight);

            var updatedOutputData = updatedPassengerDto is PassengerDetailsDto u
                ? new { u.Infant, u.SpecialServiceRequests }
                : updatedPassengerDto;

            var passengerRecord = new ActionHistory<object>(ActionTypeEnum.Created, passenger.Id, nameof(Infant),
                updatedOutputData);
            
            await _actionHistoryRepository.AddAsync(passengerRecord);
            await _passengerRepository.UpdateAsync(passenger);

            return Ok(updatedOutputData);
        }

        /// <summary>
        /// Removes the infant associated with the specified passenger ID.
        /// </summary>
        /// <param name="id">The ID of the infant.</param>
        /// <param name="flightId">The ID of the flight.</param>
        /// <returns>A <see cref='NoContentResult'/> if the operation is successful.</returns>
        [HttpDelete("infant/{id:guid}/remove-infant")]
        public async Task<ActionResult> RemoveInfant(Guid id, Guid flightId)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(flightId, false);
            var passenger = await _passengerRepository.GetPassengerByCriteriaAsync(c => c.InfantId == id);
            var bookingDetails = await _passengerBookingDetailsRepository.GetBookingDetailsByCriteriaAsync(
                               pbd => pbd.PassengerOrItemId == id);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with infant Id {id} was not found."));
            }
            
            if (flight == null)
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} was not found."));
            }
            
            passenger.InfantId = null;
            
            var oldVal = await _passengerRepository.GetPassengerByIdAsync(passenger.Id, false);
            
            await _passengerRepository.UpdateAsync(passenger);
            

            if (bookingDetails != null)
            {
                bookingDetails.PassengerOrItemId = null;
                await _passengerBookingDetailsRepository.UpdateAsync(bookingDetails);
            }

            var infant = await _infantRepository.GetInfantByIdAsync(id);

            if (infant == null)
            {
                return NotFound(new ApiResponse(404, $"Infant with Id {id} was not found."));
            }
            
            await _infantRepository.DeleteAsync(infant);
           
            var oldValDto = _passengerDtoMappingService.MapSinglePassengerOrItemToDto(oldVal, flight);
            
            var oldOutputData = oldValDto is PassengerDetailsDto o
                ? new { o.Infant, o.SpecialServiceRequests }
                : oldValDto;

            var record = new ActionHistory<object?>(ActionTypeEnum.Deleted, passenger.Id, nameof(Infant), null, 
                oldOutputData);
                
            await _actionHistoryRepository.AddAsync(record);

            return NoContent();
        }

        /// <summary>
        /// Adds a no-rec passenger to a selected flight.
        /// </summary>
        /// <param name="flightId">The ID of the selected flight.</param>
        /// <param name="model">The model containing the details of the non-recurring passenger.</param>
        /// <remarks>
        /// No-rec passengers are passengers who have not been found in the customer list of the flight but have valid
        /// booking reference on the flight.
        /// 
        /// Sample request:
        /// 
        ///     POST /passenger/selected-flight/3F2504E0-4F89-41D3-9A0C-0305E82C3301/add-no-rec-passenger
        ///     {
        ///         "firstName": "Dan",
        ///         "lastName": "Champlin",
        ///         "gender": "M",
        ///         "bookingDetailsId": "0406c369-1139-4f3b-bc4b-70679de75c48",
        ///         "baggageAllowance": 0,
        ///         "priorityBoarding": false,
        ///         "pnrId": "WSQ8JU",
        ///         "isChild": false,
        ///         "flights": {
        ///             "DY343": "16OCT",
        ///             "DY196": "16OCT"
        ///         },
        ///         "bookedClass": {
        ///             "DY343": "Y",
        ///             "DY196": "Y"
        ///         }
        ///     }
        /// </remarks>
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="PassengerDetailsDto"/> object.</returns>
        [HttpPost("passenger/selected-flight/{flightId:guid}/add-no-rec-passenger")]
        public async Task<ActionResult<PassengerDetailsDto>> AddNoRecPassenger(Guid flightId,
            [FromBody] NoRecPassengerModel model)
        {
            if (await _flightRepository.GetFlightByIdAsync(flightId, false) is not Flight selectedFlight)
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} was not found."));
            }

            if (model.Flights.All(f => f.Key != selectedFlight.ScheduledFlightId &&
                                       _timeProvider.ParseDate(f.Value) != selectedFlight.DepartureDateTime))
            {
                return BadRequest(new ApiResponse(400, "No-rec passenger must be added to the selected flight"));
            }

            if(!model.Flights.Any(f => model.BookedClass.ContainsKey(f.Key)))
            {
                return BadRequest(new ApiResponse(400, "Key value pairs in flights and bookedClass must match"));
            }
            
            if (selectedFlight.FlightStatus == FlightStatusEnum.Closed)
            {
                return BadRequest(new ApiResponse(400, "No-rec passenger cannot be added to a closed flight"));
            }

            var bookingDetails = (model.PNRId != null)
                ? await _passengerBookingDetailsRepository.GetBookingDetailsByCriteriaAsync(b =>
                    b.PNRId == model.PNRId && b.FirstName == model.FirstName && b.LastName == model.LastName &&
                    b.PassengerOrItem == null)
                : null;

            var noRecPassenger = new Passenger(model.BaggageAllowance, model.PriorityBoarding, model.FirstName,
                model.LastName, model.Gender, bookingDetails?.Id,
                bookingDetails?.Age < 12 || model.IsChild ? 35 : model.Gender == PaxGenderEnum.M ? 88 : 75);

            List<Flight> flights = new();
            Dictionary<string, FlightClassEnum> classes = new();

            if (bookingDetails != null)
            {
                foreach (var flight in bookingDetails.PNR.FlightItinerary)
                {
                    var flightToAdd = await _flightRepository.GetFlightByCriteriaAsync(f =>
                        f.ScheduledFlightId == flight.Key && f.DepartureDateTime == flight.Value);

                    if (flightToAdd == null)
                    {
                        return NotFound(new ApiResponse(404, "Flight not found with provided flight number and date"));
                    }
                    flights.Add(flightToAdd);
                }

                foreach (var flightClass in bookingDetails.BookedClass)
                {
                    classes.Add(flightClass.Key, flightClass.Value);
                }
            }
            else
            {
                foreach (var flight in model.Flights)
                {
                    var departureDateTime = _timeProvider.ParseDate(flight.Value);
                    var flightToAdd = await _flightRepository.GetFlightByCriteriaAsync(f =>
                        f.ScheduledFlightId == flight.Key && f.DepartureDateTime == departureDateTime);

                    if (flightToAdd == null)
                    {
                        return NotFound(new ApiResponse(404, "Flight not found with provided flight number and date"));
                    }
                    flights.Add(flightToAdd);
                }

                foreach (var flightClass in model.BookedClass)
                {
                    classes.Add(flightClass.Key, flightClass.Value);
                }
            }

            foreach (var flight in flights)
            {
                noRecPassenger.Flights.Add(new PassengerFlight(noRecPassenger.Id, flight.Id,
                    classes[flight.ScheduledFlightId]));
            }

            if (bookingDetails?.BookedSSR != null && bookingDetails.BookedSSR.Any(flight =>
                    flight.Value is { Count: > 0 }))
            {
                foreach (var flight in bookingDetails.BookedSSR)
                {
                    foreach (var ssr in flight.Value)
                    {
                        noRecPassenger.SpecialServiceRequests.Add(new SpecialServiceRequest(ssr[..4],
                            flights.FirstOrDefault(f => f.ScheduledFlightId == flight.Key)?.Id ?? Guid.Empty,
                            noRecPassenger.Id, ssr.Length > 4 ? ssr[4..] : string.Empty));
                    }
                }
            }

            if (bookingDetails?.ReservedSeats != null && bookingDetails.ReservedSeats.Any(flight => flight.Value.Any()))
            {
                foreach (var seatNumber in bookingDetails.ReservedSeats)
                {
                    var flight = flights.FirstOrDefault(f => f.ScheduledFlightId == seatNumber.Key);
                    var seat = await _seatRepository.GetSeatByCriteriaAsync(s =>
                        flight != null && s.SeatNumber == seatNumber.Value && s.FlightId == flight.Id);
                    if (seat != null) noRecPassenger.AssignedSeats.Add(seat);
                }
            }

            await _passengerRepository.AddAsync(noRecPassenger);

            if (bookingDetails != null)
            {
                bookingDetails.PassengerOrItemId = noRecPassenger.Id;
                await _passengerBookingDetailsRepository.UpdateAsync(bookingDetails);
            }

            var updatedPassengerForDto = 
                await _passengerRepository.GetPassengerByIdAsync(noRecPassenger.Id, false, true);

            var noRecPassengerDto = _mapper.Map<PassengerDetailsDto>(updatedPassengerForDto, opt =>
            {
                opt.Items["DepartureDateTime"] = selectedFlight.DepartureDateTime;
                opt.Items["FlightId"] = flightId;
            });
            
            var record = new ActionHistory<object>(ActionTypeEnum.Created, noRecPassenger.Id, nameof(Passenger),
                noRecPassengerDto);
                
            await _actionHistoryRepository.AddAsync(record);

            return Ok(noRecPassengerDto);
        }

        /// <summary>
        /// Check-in passenger for a selected flight.
        /// </summary>
        /// <param name="flightId">The ID of the selected flight.</param>
        /// <param name="model">The passenger check-in model.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of <see cref="object"/>.
        /// </returns>
        [HttpPut("selected-flight/{flightId:guid}/check-in-passengers")]
        public async Task<ActionResult<List<object>>> CheckInPassengers(Guid flightId,
            [FromBody] PassengerCheckInModel model)
        {
            Expression<Func<BasePassengerOrItem, bool>> criteria = c => model.PassengerIds.Contains(c.Id) &&
                                                                        (c is Passenger ||
                                                                         c is CabinBaggageRequiringSeat ||
                                                                         c is ExtraSeat);
            var passengerOrItems =
                await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(criteria);
            var flights = await _flightRepository.GetFlightsByCriteriaAsync(f => model.FlightIds.Contains(f.Id), true);

            if (await _flightRepository.GetFlightByIdAsync(flightId, false) is not Flight selectedFlight)
            {
                return NotFound(new ApiResponse(404, $"Flight {flightId} not found"));
            }

            if (!passengerOrItems.Any() || !flights.Any())
            {
                return NotFound(new ApiResponse(404, "Passenger(s) or flight(s) not found with provided ids"));
            }

            var hasInvalidPassengersOrItems = passengerOrItems.Any(p => p.Flights.Any(f2 =>
                flights.Any(f3 => f3.DepartureDateTime > f2.Flight.DepartureDateTime) &&
                !model.FlightIds.Contains(f2.FlightId) && f2.AcceptanceStatus == AcceptanceStatusEnum.NotAccepted));

            if (hasInvalidPassengersOrItems)
            {
                return BadRequest(new ApiResponse(400, 
                    "Passenger(s) can't be checked in for a future flight if they aren't checked in for the current flight"));
            }

            if (flights.Any(f => f.FlightStatus != FlightStatusEnum.Open))
            {
                return BadRequest(new ApiResponse(400, "Passenger(s) cannot be checked in for a closed flight"));
            }

            if (passengerOrItems.Any(p => p is Passenger))
            {
                var passengers = await _passengerRepository.GetPassengersByCriteriaAsync(p =>
                    model.PassengerIds.Contains(p.Id) && (p.TravelDocuments == null || !p.TravelDocuments.Any()));

                if (passengers.Count > 0) return BadRequest(new ApiResponse(400,
                    "Passenger(s) cannot be checked in without travel documents"));
            }

            var flightClassesMismatch = passengerOrItems.SelectMany(p => p.Flights)
                .Where(f => model.FlightIds.Contains(f.FlightId))
                .GroupBy(f => f.FlightId)
                .Any(g => g.Select(f => f.FlightClass).Distinct().Count() > 1);

            if (flightClassesMismatch)
            {
                return BadRequest(new ApiResponse(400,
                    "Passenger(s) flight classes on the selected flights do not match"));
            }

            var destinationsMismatch = passengerOrItems.SelectMany(p => p.Flights)
                .Where(f => model.FlightIds.Contains(f.FlightId))
                .GroupBy(f => f.FlightId)
                .Any(g => g.Select(f => f.Flight.DestinationToId).Distinct().Count() > 1);

            if (destinationsMismatch)
            {
                  return BadRequest(new ApiResponse(400,
                      "Passenger(s) final destinations on the selected flights do not match"));
            }

            foreach (var flight in flights)
            {
                var orderedSeats = flight.Seats.OrderBy(s => s.Row).ThenBy(s => s.Letter).ToList();
                var emptySeats = orderedSeats.Where(s => s.SeatStatus == SeatStatusEnum.Empty).ToList();
                var seatsToAssign = new List<Seat>();

                var passengersOrItemsWithoutSeat = passengerOrItems
                    .Where(p => p.AssignedSeats.All(s => s.FlightId != flight.Id))
                    .ToList();
                var groupSize = passengersOrItemsWithoutSeat.Count;

                if (groupSize == 1 && emptySeats.Any())
                {
                    var availableSeat = _SelectSeatForSinglePassenger(emptySeats, model);

                    seatsToAssign.Add(availableSeat);
                }
                else if (groupSize > 1 && emptySeats.Any())
                {
                    var potentialSeatGroups = _FindSeatsForGroup(orderedSeats, groupSize, false);
                    seatsToAssign = _SelectSeatsForPassengerGroup(potentialSeatGroups, orderedSeats, groupSize, model);
                }

                for (int i = 0; i < groupSize; i++)
                {
                    if (i < seatsToAssign.Count)
                    {
                        passengersOrItemsWithoutSeat[i].AssignedSeats.Add(seatsToAssign[i]);
                        seatsToAssign[i].SeatStatus = SeatStatusEnum.Occupied;
                    }
                    else
                    {
                        var passengerFlight = passengersOrItemsWithoutSeat[i]
                            .Flights.FirstOrDefault(f => f.FlightId == flight.Id);
                        if (passengerFlight != null) passengerFlight.AcceptanceStatus = AcceptanceStatusEnum.Standby;
                    }
                }

                var highestSequenceNumber =
                    await _passengerFlightRepository.GetHighestSequenceNumberOfTheFlight(flight.Id);

                foreach (var passengerOrItem in passengerOrItems)
                {
                    if (passengerOrItem.AssignedSeats.Any(s => s.FlightId == flight.Id))
                    {
                        var passengerFlight = passengerOrItem.Flights.FirstOrDefault(f => f.FlightId == flight.Id);
                        if (passengerFlight != null)
                        {
                            passengerFlight.AcceptanceStatus = AcceptanceStatusEnum.Accepted;
                            passengerFlight.BoardingSequenceNumber = ++highestSequenceNumber;

                            if (passengerOrItem is Passenger passenger)
                            {
                                await _UpdateBoardingZoneAndInfantAcceptance(passenger, flight, passengerFlight, 
                                    highestSequenceNumber);
                            }
                        }
                        else
                        {
                            return NotFound(new ApiResponse(404, "Passenger(s) not found on the selected flight"));
                        }
                    }
                    else if (passengerOrItem is CabinBaggageRequiringSeat seat)
                    {
                        seat.Weight = model.Weight;
                    }
                }
            }
            
            var oldValues =
                await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(criteria, false);

            await _basePassengerOrItemRepository.UpdateAsync(passengerOrItems.ToArray());

            var checkedInPassengerOrItemsDto =
                await _passengerHistoryService.SavePassengerOrItemActionsToPassengerHistoryAsync(oldValues,
                    passengerOrItems, selectedFlight, false);
            
            return Ok(checkedInPassengerOrItemsDto);
        }

        /// <summary>
        /// Selects a seat for a single passenger based on their seat preference and seat availability.
        /// </summary>
        /// <param name="emptySeats">The list of available seats.</param>
        /// <param name="model">The passenger check-in model containing the seat preference.</param>
        /// <returns>The selected seat for the passenger.</returns>
        private static Seat _SelectSeatForSinglePassenger(IReadOnlyCollection<Seat> emptySeats,
            PassengerCheckInModel model)
        {
            var exitSeat = emptySeats.FirstOrDefault(s => s.SeatType == SeatTypeEnum.EmergencyExit);
            if (exitSeat != null && model.SeatPreference is SeatPreferenceEnum.Exit) return exitSeat;

            var windowSeat = emptySeats.FirstOrDefault(s => s.Position == SeatPositionEnum.Window);
            if (windowSeat != null && model.SeatPreference is SeatPreferenceEnum.Window or SeatPreferenceEnum.None)
                return windowSeat;

            var aisleSeat = emptySeats.FirstOrDefault(s => s.Position == SeatPositionEnum.Aisle);
            if (aisleSeat != null && model.SeatPreference is SeatPreferenceEnum.Aisle or SeatPreferenceEnum.None)
                return aisleSeat;

            return emptySeats.First();
        }

        /// <summary>
        /// Selects seats for a passenger group based on given seat groups, ordered seats, group size, and passenger
        /// check-in model.
        /// </summary>
        /// <param name="potentialSeatGroups">A collection of seat groups that can potentially accommodate the passenger
        /// group.</param>
        /// <param name="orderedSeats">A list of all available seats in the aircraft, ordered by seat position.</param>
        /// <param name="groupSize">The total number of passengers in the group.</param>
        /// <param name="model">The passenger check-in model containing seat preference information.</param>
        /// <returns>A list of seats selected for the passenger group. The list may be empty if no suitable seats are
        /// found.</returns>
        private static List<Seat> _SelectSeatsForPassengerGroup(IReadOnlyCollection<List<Seat>> potentialSeatGroups,
            List<Seat> orderedSeats, int groupSize, PassengerCheckInModel model)
        {
            var random = new Random();
            var seatsToAssign = new List<Seat>();
            var emptySeats = orderedSeats.Where(s => s.SeatStatus == SeatStatusEnum.Empty).ToList();

            if (potentialSeatGroups.Count > 0)
            {
                
                var maxRank = potentialSeatGroups.Max(rankSeats);
                var maxRankGroups = potentialSeatGroups.Where(group => rankSeats(group) == maxRank).ToList();
                var bestSelection = maxRankGroups[random.Next(maxRankGroups.Count)];
                
                seatsToAssign.AddRange(bestSelection);
            }

            if (seatsToAssign.Count == 0)
            {
                var splitGroups = _SplitGroup(groupSize);
                foreach (var split in splitGroups)
                {
                    foreach (var subGroup in split)
                    {
                        var availableSeats = _FindSeatsForGroup(orderedSeats, subGroup, true).First();
                        if (availableSeats.Count == subGroup)
                        {
                            seatsToAssign.AddRange(availableSeats);
                        }
                    }

                    if (seatsToAssign.Count == groupSize)
                    {
                        break;
                    }

                    seatsToAssign.Clear();
                }
            }

            if (seatsToAssign.Count == 0 && emptySeats.Count > 0)
            {
                int count = Math.Min(groupSize, emptySeats.Count);
                for (int i = 0; i < count; i++)
                {
                    seatsToAssign.Add(emptySeats[i]);
                }
            }

            return seatsToAssign;

            // local function that prioritizes found groups of seats based on the given criteria
            int rankSeats(List<Seat> group)
            {
                switch (model.SeatPreference)
                {
                    case SeatPreferenceEnum.Exit:
                        return group.Any(s => s.SeatType == SeatTypeEnum.EmergencyExit) ? 4 : 3;
                    case SeatPreferenceEnum.Window:
                    case SeatPreferenceEnum.None:
                        return group.Any(s => s.Position == SeatPositionEnum.Window) ? 3 : 2;
                    case SeatPreferenceEnum.Aisle:
                        return group.Any(s => s.Position == SeatPositionEnum.Aisle) ? 2 : 1;
                    case SeatPreferenceEnum.Middle:
                    default:
                        return group.Zip(group, (a, b) => new { a = a.Position, b = b.Position })
                            .All(pair => pair.a != SeatPositionEnum.Aisle && pair.b != SeatPositionEnum.Aisle)
                            ? 1
                            : 0;
                }
            }
        }

        /// <summary>
        /// Splits a group size into subgroups.
        /// </summary>
        /// <param name="groupSize">The total size of the group.</param>
        /// <returns>A list of subgroups, where each subgroup is represented as an array of integers.</returns>
        private static List<int[]> _SplitGroup(int groupSize)
        {
            var splits = new List<int[]>();
            for (int i = groupSize - 1; i > 0; i--)
            {
                splits.Add(new[] { i, groupSize - i });
            }

            return splits;
        }

        /// <summary>
        /// Finds available seats for a group of passengers.
        /// </summary>
        /// <param name="orderedSeats">The list of ordered seats.</param>
        /// <param name="groupSize">The size of the passenger group.</param>
        /// <param name="takeFirst">A flag indicating whether to take only the first available seat group.</param>
        /// <returns>A list of available seat groups for the passenger group.</returns>
        private static List<List<Seat>> _FindSeatsForGroup(List<Seat> orderedSeats, int groupSize, bool takeFirst)
        {
            var seatGroups = new List<List<Seat>>();
            var selectedSeats = new List<Seat>(groupSize);

            foreach (var seat in orderedSeats)
            {
                selectedSeats.Add(seat);

                if (selectedSeats.Count > groupSize)
                {
                    selectedSeats.RemoveAt(0);
                }

                if (selectedSeats.Count == groupSize
                    && selectedSeats.All(s => s.SeatStatus == SeatStatusEnum.Empty)
                    && (selectedSeats.Count < 2
                        || (selectedSeats[0].Row == selectedSeats[1].Row
                            && selectedSeats[^2].Row == selectedSeats[^1].Row)))
                {
                    seatGroups.Add(new List<Seat>(selectedSeats));
                }

                if (takeFirst && seatGroups.Count > 0)
                {
                    break;
                }
            }

            return seatGroups;
        }

        /// <summary>
        /// Accepts passenger(s) from a standby list.
        /// </summary>
        /// <param name="flightId">The ID of the selected flight.</param>
        /// <param name="passengerIds">The list of passenger IDs to accept</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of <see cref="object"/>.
        /// </returns>
        [HttpPatch("selected-flight/{flightId:guid}/onload-passengers")]
        public async Task<ActionResult<List<object>>> OnloadPassengers(Guid flightId, List<Guid> passengerIds)
        {
            if(!await _flightRepository.ExistsAsync(flightId))
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} was not found."));
            }

            Expression<Func<BasePassengerOrItem, bool>> criteria = c => passengerIds.Contains(c.Id) &&
                                                                        (c is Passenger ||
                                                                         c is CabinBaggageRequiringSeat ||
                                                                         c is ExtraSeat);

            var passengerOrItems =
                await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(criteria);
            var availableSeats =
                await _seatRepository.GetSeatsByCriteriaAsync(
                    s => s.FlightId == flightId && s.SeatStatus == SeatStatusEnum.Empty);

            if (!passengerOrItems.Any())
            {
                return NotFound(new ApiResponse(404, $"Passenger(s) or flight not found with provided ids"));
            }

            if (await _flightRepository.GetFlightByIdAsync(flightId, false) is not Flight flight)
            {
                return NotFound(new ApiResponse(404, $"Flight {flightId} not found"));
            }

            if (flight.FlightStatus == FlightStatusEnum.Finalised)
            {
                return BadRequest(new ApiResponse(400,
                    "Onload of selected passenger(s) can't be done as the flight is finalised"));
            }

            if (availableSeats.Count == 0)
            {
                return Ok(new ApiResponse(200, "No available seats found on the current flight"));
            }

            var highestSequenceNumber = await _passengerFlightRepository.GetHighestSequenceNumberOfTheFlight(flight.Id);

            foreach (var passengerOrItem in passengerOrItems)
            {
                var passengerFlight = passengerOrItem.Flights.FirstOrDefault(f => f.FlightId == flight.Id);
                if (passengerFlight != null)
                {
                    if (passengerFlight.AcceptanceStatus != AcceptanceStatusEnum.Standby)
                    {
                        return BadRequest(new ApiResponse(400,
                            "Onload of the passenger can't be done as the passenger is not on standby"));
                    }

                    var availableSeat = availableSeats.FirstOrDefault(s =>
                        s.FlightClass == passengerFlight.FlightClass && s.SeatStatus == SeatStatusEnum.Empty);

                    if (availableSeat != null)
                    {
                        passengerOrItem.AssignedSeats.Add(availableSeat);
                        availableSeat.SeatStatus = SeatStatusEnum.Occupied;
                        passengerFlight.AcceptanceStatus = AcceptanceStatusEnum.Accepted;
                        passengerFlight.BoardingSequenceNumber = ++highestSequenceNumber;

                        if (passengerOrItem is Passenger passenger)
                        {
                            await _UpdateBoardingZoneAndInfantAcceptance(passenger, flight, passengerFlight, 
                                highestSequenceNumber);
                        }
                    }
                    else
                    {
                        return Ok(new ApiResponse(200,
                            "No seats available for the passenger(s) in required flight class"));
                    }
                }
                else
                {
                    return NotFound(new ApiResponse(404, "Passenger(s) not found on the selected flight"));
                }
            }
            
            var oldValues =
                await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(criteria, false);

            await _passengerRepository.UpdateAsync(passengerOrItems.ToArray());
            
            var onloadedPassengerOrItemsDto = 
                await _passengerHistoryService.SavePassengerOrItemActionsToPassengerHistoryAsync(oldValues,
                    passengerOrItems, flight, false);
            
            return Ok(onloadedPassengerOrItemsDto);
        }

        /// <summary>
        /// Updates the boarding zone and infant acceptance status for a passenger.
        /// </summary>
        /// <param name="passenger">The passenger to update.</param>
        /// <param name="flight">The flight the passenger is associated with.</param>
        /// <param name="passengerFlight">The join class linking the passenger and flight.</param>
        /// <param name="highestSequenceNumber">The highest sequence number among all passengers on the flight.</param>
        private async Task _UpdateBoardingZoneAndInfantAcceptance(Passenger passenger, BaseFlight flight, 
            PassengerFlight passengerFlight, int highestSequenceNumber)
        {
            passengerFlight.BoardingZone = passenger.PriorityBoarding
                ? BoardingZoneEnum.A : passenger.BaggageAllowance > 0
                ? BoardingZoneEnum.B : BoardingZoneEnum.C;

            if (passenger.InfantId != null)
            {
                var infant = await _infantRepository.GetInfantByIdAsync(passenger.InfantId.Value);

                var infantFlight = infant.Flights.FirstOrDefault(f => f.FlightId == flight.Id);
                if (infantFlight != null)
                {
                    infantFlight.AcceptanceStatus = AcceptanceStatusEnum.Accepted;
                    infantFlight.BoardingSequenceNumber = highestSequenceNumber;
                }
            }
        }

        /// <summary>
        /// Cancels the acceptance of passenger(s) for a selected flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight for which the passenger acceptance needs to be cancelled.
        /// </param>
        /// <param name="model">The passenger check-in model containing the IDs of the passengers to cancel acceptance
        /// for.</param>
        /// <param name="updateStatusTo">The acceptance status to update the passengers to after cancellation.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of <see cref="object"/>.
        /// </returns>
        [HttpPut("selected-flight/{flightId:guid}/cancel-passenger-acceptance")]
        public async Task<ActionResult<List<object>>> CancelPassengerAcceptance(Guid flightId,
            [FromBody] PassengerCheckInModel model, AcceptanceStatusEnum updateStatusTo)
        {
            Expression<Func<BasePassengerOrItem, bool>> criteria = c => model.PassengerIds.Contains(c.Id) &&
                                                                        (c is Passenger ||
                                                                         c is CabinBaggageRequiringSeat ||
                                                                         c is ExtraSeat);

            var passengerOrItems =
                await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(criteria);
            var flights = await _flightRepository.GetFlightsByCriteriaAsync(f => model.FlightIds.Contains(f.Id));

            if (await _flightRepository.GetFlightByIdAsync(flightId, false) is not Flight selectedFlight)
            {
                return NotFound(new ApiResponse(404, $"Flight {flightId} not found"));
            }

            if (!passengerOrItems.Any() || !flights.Any())
            {
                return NotFound(new ApiResponse(404, $"Passenger(s) or flight(s) not found with provided ids"));
            }

            var hasInvalidPassengersOrItems = passengerOrItems.Any(p => p.Flights.Any(f2 =>
                flights.Any(f3 => f3.DepartureDateTime < f2.Flight.DepartureDateTime) &&
                !model.FlightIds.Contains(f2.FlightId) && f2.AcceptanceStatus == AcceptanceStatusEnum.Accepted));

            if (hasInvalidPassengersOrItems)
            {
                return BadRequest(new ApiResponse(400,
                    "Acceptance of selected passenger(s) and flights can't be cancelled as some of them are already " +
                    "checked in for onward flights"));
            }

            if (selectedFlight.FlightStatus == FlightStatusEnum.Finalised)
            {
                return BadRequest(new ApiResponse(400,
                    "Acceptance of selected passenger(s) can't be cancelled as the flight is finalised"));
            }

            foreach (var flight in flights)
            {
                foreach (var passengerOrItem in passengerOrItems)
                {
                    var passengerFlight = passengerOrItem.Flights.FirstOrDefault(f => f.FlightId == flight.Id);

                    if (passengerFlight == null)
                    {
                        return NotFound(new ApiResponse(404, "Passenger(s) not found on the selected flight"));
                    }

                    if (passengerFlight.AcceptanceStatus == AcceptanceStatusEnum.Boarded)
                    {
                        return BadRequest(new ApiResponse(400,
                            "Acceptance of the passenger can't be cancelled as the passenger is already boarded"));
                    }

                    if (passengerFlight.AcceptanceStatus != updateStatusTo)
                    {
                        _ProcessPassengerFlightStatusChange(passengerFlight, passengerOrItem, flight.Id, updateStatusTo,
                            model);

                        var onwardFlightsWithAcceptedStatusNotIncludedInSelection = passengerOrItem.Flights.Where(f =>
                            f.Flight.DepartureDateTime > flight.DepartureDateTime &&
                            model.FlightIds.All(f2 => f2 != f.FlightId) &&
                            f.AcceptanceStatus is AcceptanceStatusEnum.Accepted or AcceptanceStatusEnum.Standby);

                        foreach (var onwardFlight in onwardFlightsWithAcceptedStatusNotIncludedInSelection)
                        {
                            _ProcessPassengerFlightStatusChange(onwardFlight, passengerOrItem, flight.Id,
                                updateStatusTo, model);
                        }
                    }
                }
            }
            
            var oldValues =
                await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(criteria, false);

            await _passengerRepository.UpdateAsync(passengerOrItems.ToArray());
           
            var cancelledPassengerOrItemsDto = 
                await _passengerHistoryService.SavePassengerOrItemActionsToPassengerHistoryAsync(oldValues,
                    passengerOrItems, selectedFlight);

            return Ok(cancelledPassengerOrItemsDto);
        }

        /// <summary>
        /// Processes the change in passenger flight status.
        /// </summary>
        /// <param name="passengerFlight">The passenger flight object.</param>
        /// <param name="passengerOrItem">The base passenger or item object.</param>
        /// <param name="flightId">The flight ID.</param>
        /// <param name="updateStatusTo">The updated acceptance status.</param>
        /// <param name="model">The passenger check-in model.</param>
        private static void _ProcessPassengerFlightStatusChange(PassengerFlight passengerFlight,
            BasePassengerOrItem passengerOrItem, Guid flightId, AcceptanceStatusEnum updateStatusTo,
            PassengerCheckInModel model)
        {
            passengerFlight.AcceptanceStatus = updateStatusTo;
            passengerFlight.BoardingSequenceNumber = null;
            passengerFlight.BoardingZone = null;

            switch (updateStatusTo)
            {
                case AcceptanceStatusEnum.Standby:
                    passengerOrItem.AssignedSeats.First(s => s.FlightId == flightId).SeatStatus =
                        SeatStatusEnum.Empty;
                    passengerOrItem.AssignedSeats.RemoveAll(s => s.FlightId == flightId);
                    break;
                case AcceptanceStatusEnum.NotTravelling:
                    passengerOrItem.AssignedSeats.First(s => s.FlightId == flightId).SeatStatus =
                        SeatStatusEnum.Empty;
                    passengerOrItem.AssignedSeats.RemoveAll(s => s.FlightId == flightId);
                    passengerFlight.NotTravellingReason = model.NotTravellingReason;
                    break;
            }
        }

        /// <summary>
        /// Retrieves the action history for a specific passenger.
        /// </summary>
        /// <param name="id">The ID of the passenger to retrieve the action history for.</param>
        /// <returns>A list of ActionHistoryDto objects representing the action history for the passenger.</returns>
        [HttpGet("{id:guid}/history")]
        public async Task<ActionResult<List<ActionHistoryDto>>> ShowPassengerHistory(Guid id)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id, false, true);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            var passengerHistoryList = await _actionHistoryRepository.GetActionHistoryByPassengerId(id);
            
            var passengerHistoryDtoList = _mapper.Map<List<ActionHistoryDto>>(passengerHistoryList);

            return Ok(passengerHistoryDtoList);
        }

        /// <summary>
        /// Updates the acceptance status of passengers on a flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="model">The model containing the passenger and boarding information.</param>
        /// <param name="updateStatusTo">The acceptance status to update the passengers to.</param>
        /// <param name="errorMessage">The error message to return if no passenger is found.</param>
        /// <returns>An ActionResult containing a list of updated objects or an error response.</returns>
        private async Task<ActionResult<List<object>>> UpdateBoardStatus(Guid flightId, BoardPassengerModel model,
            AcceptanceStatusEnum updateStatusTo, string errorMessage)
        {
            if (await _flightRepository.GetFlightByIdAsync(flightId, false) is not Flight flight)
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} was not found."));
            }

            var checkAcceptanceStatus = updateStatusTo == AcceptanceStatusEnum.Accepted
                ? AcceptanceStatusEnum.Boarded
                : AcceptanceStatusEnum.Accepted;

            Expression<Func<BasePassengerOrItem, bool>> criteria = c =>
                c.AssignedSeats.Any(s => s.SeatNumber == model.SeatNumber && s.FlightId == flightId) || c.Flights.Any(
                    f => f.FlightId == flightId && f.BoardingSequenceNumber == model.BoardingSequenceNumber &&
                         f.AcceptanceStatus == checkAcceptanceStatus);

            var passengerOrItem = await _basePassengerOrItemRepository.GetBasePassengerOrItemByCriteriaAsync(criteria);

            if (passengerOrItem == null)
            {
                return NotFound(new ApiResponse(404, errorMessage));
            }

            if (flight.BoardingStatus != BoardingStatusEnum.Open)
            {
                return BadRequest(new ApiResponse(400, "Passenger(s) cannot be processed when boarding is closed"));
            }

            passengerOrItem.Flights.First(f => f.FlightId == flightId).AcceptanceStatus = updateStatusTo;

            var oldVal = await _basePassengerOrItemRepository.GetBasePassengerOrItemByIdAsync(passengerOrItem.Id);

            await _basePassengerOrItemRepository.UpdateAsync(passengerOrItem);

            var updatedPassengerOrItemDto =
                await _passengerHistoryService.SavePassengerOrItemActionsToPassengerHistoryAsync(
                    new List<BasePassengerOrItem> { oldVal }, new List<BasePassengerOrItem> { passengerOrItem }, flight,
                    false);

            return Ok(updatedPassengerOrItemDto);
        }

        /// <summary>
        /// Updates the boarding status of a passenger for a selected flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="model">The model containing the passenger details.</param>
        /// <returns>
        /// Returns a list of objects representing the updated passenger details.
        /// </returns>
        [HttpPatch("selected-flight/{flightId:guid}/board-passengers")]
        public async Task<ActionResult<List<object>>> BoardPassenger(Guid flightId,
            [FromBody] BoardPassengerModel model)
        {
            return await UpdateBoardStatus(flightId, model, AcceptanceStatusEnum.Boarded,
                "No checked in passenger was found with provided seat number or boarding sequence number");
        }

        /// <summary>
        /// Deboards a passenger from a flight.
        /// </summary>
        /// <param name="flightId">The unique identifier of the flight.</param>
        /// <param name="model">The model containing the details of the passenger to be deboarded.</param>
        /// <returns>A list of objects representing the updated board status of the deboarded passenger.</returns>
        [HttpPatch("selected-flight/{flightId:guid}/deboard-passengers")]
        public async Task<ActionResult<List<object>>> DeboardPassenger(Guid flightId,
            [FromBody] BoardPassengerModel model)
        {
            return await UpdateBoardStatus(flightId, model, AcceptanceStatusEnum.Accepted,
                "No boarded passenger was found with provided seat number or boarding sequence number");
        }
    }
}