using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Core.BaggageContext;
using Core.Dtos;
using Core.FlightContext;
using Core.FlightContext.FlightInfo.Enums;
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

        public PassengerController(
            IMapper mapper,
            ITimeProvider timeProvider,
            IFlightRepository flightRepository,
            IPassengerRepository passengerRepository,
            IInfantRepository infantRepository,
            IPassengerBookingDetailsRepository passengerBookingDetailsRepository,
            IPassengerFlightRepository passengerFlightRepository,
            ISeatRepository seatRepository,
            IBasePassengerOrItemRepository basePassengerOrItemRepository)
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
        ///         "departureDate": "18OCT"
        ///         "destinationFrom": "OSL",
        ///         "destinationTo": "LHR",
        ///         "documentNumber": "12345789",
        ///         "lastName": "Doe",
        ///         "pnr": "ABC123",
        ///         "seatNumber": "1A"
        ///     }
        ///
        /// </remarks> 
        /// <returns>Returns a list of Passenger objects that match the search criteria.</returns>
        [HttpPost("search-passengers")]
        public async Task<ActionResult<List<PassengerOverviewDto>>> SearchPassengers([FromBody] JObject data)
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
        public async Task<ActionResult<PassengerDetailsDto>> GetPassengerDetails(Guid flightId,
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
        public async Task<ActionResult<List<PassengerOverviewDto>>> GetPassengersByBookingReference(Guid flightId,
            string bookingReference)
        {
            //ToDo : Include to search also ExtraSeat and CabinBaggageRequiringSeat
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
        public async Task<ActionResult<List<BaggageDetailsDto>>> GetAllPassengersBags(Guid id)
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
        public async Task<ActionResult<List<PassengerDetailsDto>>> AddPassengersToSelection(Guid flightId,
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

            var existingPassengerList = new List<Passenger>(existingPassengers);
            existingPassengerList.AddRange(passengersToAdd);

            var passengersDto = existingPassengerList.Select(passenger => _mapper.Map<PassengerDetailsDto>(
                passenger, opt =>
                {
                    opt.Items["DepartureDateTime"] = selectedFlight.DepartureDateTime;
                    opt.Items["FlightId"] = flightId;
                }));

            return Ok(passengersDto);
        }

        /// <summary>
        /// Adds an infant to a passenger.
        /// </summary>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="infantModel">The details of the infant to be added.</param>
        /// <returns>An ActionResult with the Infant object if the operation is successful; otherwise, an appropriate
        /// status code is returned.</returns>
        [HttpPost("passenger/{id:guid}/add-infant")]
        public async Task<ActionResult<Infant>> AddInfant(Guid id,
            [FromBody] InfantModel infantModel)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id);

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

            var infant = new Infant(id, infantModel.FirstName, infantModel.LastName, infantModel.Gender,
                passenger.BookingDetails.AssociatedPassengerBookingDetailsId, 0);

            foreach (var flight in passenger.Flights)
            {
                infant.Flights.Add(new PassengerFlight(infant.Id, flight.FlightId, flight.FlightClass));
            }

            await _infantRepository.AddAsync(infant);
            passenger.InfantId = infant.Id;

            Expression<Func<PassengerBookingDetails, bool>> criteria = c => c.Id == infant.BookingDetailsId;
            var bookingDetails = await _passengerBookingDetailsRepository.GetBookingDetailsByCriteriaAsync(criteria);

            if (bookingDetails != null)
            {
                bookingDetails.PassengerId = infant.Id;
                await _passengerBookingDetailsRepository.UpdateAsync(bookingDetails);
            }

            await _passengerRepository.UpdateAsync(passenger);

            return Ok();
        }

        /// <summary>
        /// Removes the infant associated with the specified passenger ID.
        /// </summary>
        /// <param name="id">The ID of the passenger.</param>
        /// <returns>An empty response if the infant was successfully removed, or a 404 error response if the infant
        /// was not found.</returns>
        [HttpDelete("passenger/{id:guid}/remove-infant")]
        public async Task<ActionResult<Passenger>> RemoveInfant(Guid id)
        {
            Expression<Func<Passenger, bool>> criteria = c => c.InfantId == id;
            var passenger = await _passengerRepository.GetPassengerByCriteriaAsync(criteria);
            var bookingDetails = await _passengerBookingDetailsRepository.GetBookingDetailsByCriteriaAsync(
                               pbd => pbd.PassengerId == id);

            if (passenger != null)
            {
                passenger.InfantId = null;
                await _passengerRepository.UpdateAsync(passenger);
            }

            if (bookingDetails != null)
            {
                bookingDetails.PassengerId = null;
                await _passengerBookingDetailsRepository.UpdateAsync(bookingDetails);
            }

            var infant = await _infantRepository.GetInfantByIdAsync(id);

            if (infant == null)
            {
                return NotFound(new ApiResponse(404, $"Infant with Id {id} was not found."));
            }

            await _infantRepository.DeleteAsync(infant);

            return Ok();
        }

        /// <summary>
        /// Adds a no-rec passenger to a selected flight. No-rec passengers are passengers who have not been found
        /// in the customer list of the flight but have valid booking reference on the flight.
        /// </summary>
        /// <param name="flightId">The ID of the selected flight.</param>
        /// <param name="model">The model containing the details of the non-recurring passenger.</param>
        /// <remarks>
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
        /// <returns>An ActionResult representing the result of the operation.</returns>
        [HttpPost("passenger/selected-flight/{flightId:guid}/add-no-rec-passenger")]
        public async Task<ActionResult<Passenger>> AddNoRecPassenger(Guid flightId,
            [FromBody] NoRecPassengerModel model)
        {
            var selectedFlight = await _flightRepository.GetFlightByIdAsync(flightId, false);

            if (selectedFlight == null)
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} was not found."));
            }

            var bookingDetails = (model.PNRId != null)
                ? await _passengerBookingDetailsRepository.GetBookingDetailsByCriteriaAsync(b =>
                    b.PNRId == model.PNRId && b.FirstName == model.FirstName && b.LastName == model.LastName &&
                    b.Passenger == null)
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

                    if (flightToAdd != null)
                    {
                        flights.Add(flightToAdd);
                    }
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

                    if (flightToAdd != null)
                    {
                        flights.Add(flightToAdd);
                    }
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
                foreach (var seat in bookingDetails.ReservedSeats)
                {
                    //...
                }
            }

            await _passengerRepository.AddAsync(noRecPassenger);

            if (bookingDetails != null)
            {
                bookingDetails.PassengerId = noRecPassenger.Id;
                await _passengerBookingDetailsRepository.UpdateAsync(bookingDetails);
            }

            return Ok();
        }

        [HttpPut("selected-flight/{flightId:guid}/check-in-passenger")]
        public async Task<ActionResult<BasePassengerOrItem>> CheckInPassenger(Guid flightId, [FromBody] PassengerCheckInModel model)
        {
            Expression<Func<BasePassengerOrItem, bool>> criteria = c => model.PassengerIds.Contains(c.Id) &&
                (c is Passenger || c is CabinBaggageRequiringSeat || c is ExtraSeat);

            var passengersOrItems = await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(criteria);
            var flights = await _flightRepository.GetFlightsByCriteriaAsync(f => model.FlightIds.Contains(f.Id), false);
            var selectedFlight = await _flightRepository.GetFlightByIdAsync(flightId, false) as Flight;

            if (passengersOrItems == null || flights == null || selectedFlight == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger(s) or flight(s) not found with provided ids"));
            }

            var selectedFlightDeparture = selectedFlight.DepartureDateTime;

            var hasInvalidPassengersOrItems = passengersOrItems.Any(p =>
                p.Flights.Any(f2 =>
                    flights.Any(f3 => f3.DepartureDateTime > f2.Flight.DepartureDateTime) &&
                    !model.FlightIds.Contains(f2.FlightId) &&
                    f2.AcceptanceStatus == AcceptanceStatusEnum.NotAccepted)
            );

            if (hasInvalidPassengersOrItems)
            {
                return BadRequest(new ApiResponse(400,
                    "Passenger(s) cannot be checked in for a future flight if they are not checked in for the current flight"));
            }

            if (selectedFlight.FlightStatus != FlightStatusEnum.Open)
            {
                return BadRequest(new ApiResponse(400, "Passenger(s) cannot be checked in for a closed flight"));
            }

            if (passengersOrItems.Any(p => p is Passenger && (p.TravelDocuments == null || !p.TravelDocuments.Any())))
            {
                return BadRequest(new ApiResponse(400, "Passenger(s) cannot be checked in without travel documents"));
            }

            var flightClassesMismatch = passengersOrItems
                .SelectMany(p => p.Flights)
                .Where(f => model.FlightIds.Contains(f.FlightId))
                .GroupBy(f => f.FlightId)
                .Any(g => g.Select(f => f.FlightClass).Distinct().Count() > 1);

            if (flightClassesMismatch)
            {
                return BadRequest(new ApiResponse(400, "Passenger(s) flight classes on the selected flights do not match"));
            }

            foreach (var flight in flights)
            {
                var orderedSeats = flight.Seats.OrderBy(s => s.Row).ThenBy(s => s.Letter).ToList();
                var emptySeats = orderedSeats.Where(s => s.SeatStatus == SeatStatusEnum.Empty).ToList();
                var seatsToAssign = new List<Seat>();

                var passengersOrItemsWithoutSeat = passengersOrItems.Where(p => p.AssignedSeats.All(s => s.FlightId != flight.Id)).ToList();
                var passengersOrItemsWithSeats = passengersOrItems.Except(passengersOrItemsWithoutSeat).ToList();
                var groupSize = passengersOrItemsWithoutSeat.Count;

                if (groupSize == 1)
                {
                    var availableSeat = _SelectSeatForSinglePassenger(emptySeats);
                    if (availableSeat != null)
                    {
                        seatsToAssign.Add(availableSeat);
                    }
                }
                else
                {
                    var potentialSeatGroups = _FindSeatsForGroup(orderedSeats, groupSize, false);
                    seatsToAssign = _SelectSeatsForPassengerGroup(potentialSeatGroups, orderedSeats, groupSize);
                }

                for (int i = 0; i < groupSize; i++)
                {
                    if (seatsToAssign[i] == null)
                    {
                        passengersOrItemsWithoutSeat[i].Flights.FirstOrDefault(f => f.FlightId == flight.Id).AcceptanceStatus = AcceptanceStatusEnum.Standby;
                    }
                    passengersOrItemsWithoutSeat[i].AssignedSeats.Add(seatsToAssign[i]);
                }

                var highestSequenceNumber = await _passengerFlightRepository.GetHighestSequenceNumberOfTheFlight(flight.Id);

                foreach (var passengerOrItem in passengersOrItems)
                {
                    if (passengerOrItem.AssignedSeats.Any(s => s.FlightId == flight.Id))
                    {
                        passengerOrItem.Flights.FirstOrDefault(f => f.FlightId == flight.Id).AcceptanceStatus = AcceptanceStatusEnum.Accepted;
                        passengerOrItem.Flights.FirstOrDefault(f => f.FlightId == flight.Id).BoardingSequenceNumber = ++highestSequenceNumber;

                        if (passengerOrItem is Passenger)
                        {
                            passengerOrItem.Flights.FirstOrDefault(f => f.FlightId == flight.Id).BoardingZone = (passengerOrItem as Passenger).PriorityBoarding
                                ? BoardingZoneEnum.A : (passengerOrItem as Passenger).BaggageAllowance > 0
                                ? BoardingZoneEnum.B : BoardingZoneEnum.C;

                            if ((passengerOrItem as Passenger).InfantId != null)
                            {
                                var infant = await _infantRepository.GetInfantByIdAsync((passengerOrItem as Passenger).InfantId.Value);
                                infant.Flights.FirstOrDefault(f => f.FlightId == flight.Id).AcceptanceStatus = AcceptanceStatusEnum.Accepted;
                                infant.Flights.FirstOrDefault(f => f.FlightId == flight.Id).BoardingSequenceNumber = highestSequenceNumber;
                            }
                        }
                        else if (passengerOrItem is CabinBaggageRequiringSeat)
                        {
                            (passengerOrItem as CabinBaggageRequiringSeat).Weight = model.Weight;
                        }
                    }
                }
            }

            await _passengerRepository.UpdateAsync(passengersOrItems.ToArray());

            return Ok();
        }

        private Seat _SelectSeatForSinglePassenger(List<Seat> emptySeats)
        {
            if (emptySeats.Any())
            {
                var windowSeat = emptySeats.FirstOrDefault(s => s.Position == SeatPositionEnum.Window);
                if (windowSeat != null) return windowSeat;

                var aisleSeat = emptySeats.FirstOrDefault(s => s.Position == SeatPositionEnum.Aisle);
                if (aisleSeat != null) return aisleSeat;

                return emptySeats.First();
            }

            return null;
        }

        private List<Seat> _SelectSeatsForPassengerGroup(List<List<Seat>> potentialSeatGroups, List<Seat> orderedSeats, int groupSize)
        {
            var availableSeats = new List<Seat>();
            var seatsToAssign = new List<Seat>();
            var emptySeats = orderedSeats.Where(s => s.SeatStatus == SeatStatusEnum.Empty).ToList();

            if (potentialSeatGroups.Count > 0)
            {
                var bestSelection = potentialSeatGroups.OrderByDescending(g =>
                    g.Any(s => s.Position == SeatPositionEnum.Window) ? 3 :
                    g.Any(s => s.Position == SeatPositionEnum.Aisle) ? 2 :
                    g.Zip(g, (a, b) => new { a = a.Position, b = b.Position }).All(pair =>
                        pair.a != SeatPositionEnum.Aisle && pair.b != SeatPositionEnum.Aisle) ? 1 : 0
                ).FirstOrDefault();

                if (bestSelection != null)
                {
                    seatsToAssign.AddRange(bestSelection);
                }
            }

            if (seatsToAssign.Count == 0)
            {
                var splitGroups = _SplitGroup(groupSize);
                foreach (var split in splitGroups)
                {
                    foreach (var subGroup in split)
                    {
                        availableSeats = _FindSeatsForGroup(orderedSeats, subGroup, true).First();
                        if (availableSeats.Count == subGroup)
                        {
                            seatsToAssign.AddRange(availableSeats);
                        }
                    }
                    if (seatsToAssign.Count == groupSize)
                    {
                        break;
                    }
                    else
                    {
                        seatsToAssign.Clear();
                    }
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
        }

        private List<int[]> _SplitGroup(int groupSize)
        {
            List<int[]> splits = new List<int[]>();
            for (int i = groupSize - 1; i > 0; i--)
            {
                splits.Add(new int[] { i, groupSize - i });
            }

            return splits;
        }

        private List<List<Seat>> _FindSeatsForGroup(List<Seat> orderedSeats, int groupSize, bool takeFirst)
        {
            List<List<Seat>> seatGroups = new List<List<Seat>>();
            List<Seat> selectedSeats = new List<Seat>(groupSize);

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
                            && selectedSeats[selectedSeats.Count - 2].Row == selectedSeats[selectedSeats.Count - 1].Row)))
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

        [HttpPut("selected-flight/{flightId:guid}/cancel-passenger-acceptance")]
        public async Task<ActionResult<BasePassengerOrItem>> CancelPassengerAcceptance(Guid flightId, [FromBody] PassengerCheckInModel model,
            AcceptanceStatusEnum updateStatusTo)
        {
            Expression<Func<BasePassengerOrItem, bool>> criteria = c => model.PassengerIds.Contains(c.Id) &&
                (c is Passenger || c is CabinBaggageRequiringSeat || c is ExtraSeat);

            var passengersOrItems = await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(criteria);
            var flights = await _flightRepository.GetFlightsByCriteriaAsync(f => model.FlightIds.Contains(f.Id), false);
            var selectedFlight = await _flightRepository.GetFlightByIdAsync(flightId, false) as Flight;

            if (passengersOrItems == null || flights == null || selectedFlight == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger(s) or flight(s) not found with provided ids"));
            }

            var selectedFlightDeparture = selectedFlight.DepartureDateTime;

            var hasInvalidPassengersOrItems = passengersOrItems.Any(p =>
                p.Flights.Any(f2 =>
                    flights.Any(f3 => f3.DepartureDateTime < f2.Flight.DepartureDateTime) &&
                    !model.FlightIds.Contains(f2.FlightId) &&
                    f2.AcceptanceStatus == AcceptanceStatusEnum.Accepted)
            );

            if (hasInvalidPassengersOrItems)
            {
                return BadRequest(new ApiResponse(400,

                    "Acceptance of selected passenger(s) and flights can't be cancelled as some of them are already checked in for onward flights"));
            }

            if (selectedFlight.FlightStatus == FlightStatusEnum.Finalised)
            {
                return BadRequest(new ApiResponse(400, "Acceptance of selected passenger(s) can't be cancelled as the flight is finalised"));
            }

            if (passengersOrItems.Any(p => p.Flights.FirstOrDefault(f => f.FlightId == flightId).AcceptanceStatus == AcceptanceStatusEnum.Boarded))
            {
                return BadRequest(new ApiResponse(400, "Acceptance of selected passenger(s) can't be cancelled as some of them are already boarded"));
            }

            foreach (var flight in flights)
            {
                foreach (var passengerOrItem in passengersOrItems)
                {
                    var passengerFlight = passengerOrItem.Flights.FirstOrDefault(f => f.FlightId == flight.Id);
                    if (passengerFlight != null)
                    {
                        passengerFlight.AcceptanceStatus = updateStatusTo;
                        passengerFlight.BoardingSequenceNumber = null;
                        passengerFlight.BoardingZone = null;

                        switch (updateStatusTo)
                        {
                            case AcceptanceStatusEnum.Standby:
                                passengerOrItem.AssignedSeats.RemoveAll(s => s.FlightId == flight.Id);
                                break;
                            case AcceptanceStatusEnum.NotTravelling:
                                passengerOrItem.AssignedSeats.RemoveAll(s => s.FlightId == flight.Id);
                                //ToDo : Create class with offload reasons
                                break;
                        }
                    }
                    else
                    {
                        return NotFound(new ApiResponse(404, "Passenger(s) not found on the selected flight"));
                    }
                }
            }

            await _passengerRepository.UpdateAsync(passengersOrItems.ToArray());

            return Ok();
        }

        [HttpPatch("selected-flight/{flightId:guid}/onload-passenger")]
        public async Task<ActionResult<Passenger>> OnloadPassenger(Guid flightId, List<Guid> passengerIds)
        {
            Expression<Func<BasePassengerOrItem, bool>> criteria = c => passengerIds.Contains(c.Id) &&
                (c is Passenger || c is CabinBaggageRequiringSeat || c is ExtraSeat);

            var passengersOrItems = await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(criteria);
            var availableSeats = await _seatRepository.GetSeatsByCriteriaAsync(s => s.FlightId == flightId && s.SeatStatus == SeatStatusEnum.Empty, false);
            var flight = await _flightRepository.GetFlightByIdAsync(flightId, false) as Flight;

            if (passengersOrItems == null || flight == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger(s) or flight not found with provided ids"));
            }

            if (flight.FlightStatus == FlightStatusEnum.Finalised)
            {
                return BadRequest(new ApiResponse(400, "Onload of selected passenger(s) can't be done as the flight is finalised"));
            }

            if (passengersOrItems.Any(p => p.Flights.FirstOrDefault(f => f.FlightId == flightId).AcceptanceStatus != AcceptanceStatusEnum.Standby))
            {
                return BadRequest(new ApiResponse(400, "Onload of selected passenger(s) can't be done as some of them are not on standby"));
            }

            if (availableSeats == null)
            {
                return NotFound(new ApiResponse(404, "No available seats found on the current flight"));
            }

            var highestSequenceNumber = await _passengerFlightRepository.GetHighestSequenceNumberOfTheFlight(flight.Id);

            foreach (var passengerOrItem in passengers)
            {
                var passengerFlight = passengerOrItem.Flights.FirstOrDefault(f => f.FlightId == flight.Id);
                if (passengerFlight != null)
                {
                    var availableSeat = availableSeats.FirstOrDefault(s => s.FlightClass == passengerFlight.FlightClass);
                    if (availableSeat != null)
                    {
                        passengerOrItem.AssignedSeats.Add(availableSeat);
                        passengerFlight.AcceptanceStatus = AcceptanceStatusEnum.Accepted;
                        passengerFlight.BoardingSequenceNumber = ++highestSequenceNumber;
                        passengerFlight.BoardingZone = passengerOrItem.PriorityBoarding
                            ? BoardingZoneEnum.A : passengerOrItem.BaggageAllowance > 0
                            ? BoardingZoneEnum.B : BoardingZoneEnum.C;
                    }
                    else
                    {
                        return NotFound(new ApiResponse(404,
                            "No seats available for the passenger(s) in required flight class"));
                    }
                }
                else
                {
                    return NotFound(new ApiResponse(404, "Passenger(s) not found on the selected flight"));
                }
            }

            await _passengerRepository.UpdateAsync(passengers.ToArray());

            return Ok();
        }


        //[HttpPut("selected-flight/{flightId:guid}/check-in-passenger")]
        //public async Task<ActionResult<Passenger>> CheckInPassenger(Guid flightId, [FromBody] PassengerCheckInModel model)
        //{
        //    var passengers = await _passengerRepository.GetPassengersByCriteriaAsync(p => model.PassengerIds.Contains(p.Id), true, false);
        //    var flights = await _flightRepository.GetFlightsByCriteriaAsync(f => model.FlightIds.Contains(f.Id), false);
        //    var selectedFlight = await _flightRepository.GetFlightByIdAsync(flightId, false) as Flight;

        //    if (passengers == null || flights == null || selectedFlight == null)
        //    {
        //        return NotFound(new ApiResponse(404, $"Passenger(s) or flight(s) not found with provided ids"));
        //    }

        //    var selectedFlightDeparture = selectedFlight.DepartureDateTime;

        //    var hasInvalidPassengers = passengers.Any(p =>
        //        p.Flights.Any(f2 =>
        //            flights.Any(f3 => f3.DepartureDateTime > f2.Flight.DepartureDateTime) &&
        //            !model.FlightIds.Contains(f2.FlightId) &&
        //            f2.AcceptanceStatus == AcceptanceStatusEnum.NotAccepted)
        //    );

        //    if (hasInvalidPassengers)
        //    {
        //        return BadRequest(new ApiResponse(400,
        //            "Passenger(s) cannot be checked in for a future flight if they are not checked in for the current flight"));
        //    }

        //    if (selectedFlight.FlightStatus != FlightStatusEnum.Open)
        //    {
        //        return BadRequest(new ApiResponse(400, "Passenger(s) cannot be checked in for a closed flight"));
        //    }

        //    if (passengers.Any(p => p.TravelDocuments == null || !p.TravelDocuments.Any()))
        //    {
        //        return BadRequest(new ApiResponse(400, "Passenger(s) cannot be checked in without travel documents"));
        //    }

        //    var flightClassesMismatch = passengers
        //        .SelectMany(p => p.Flights)
        //        .Where(f => model.FlightIds.Contains(f.FlightId))
        //        .GroupBy(f => f.FlightId)
        //        .Any(g => g.Select(f => f.FlightClass).Distinct().Count() > 1);

        //    if (flightClassesMismatch)
        //    {
        //        return BadRequest(new ApiResponse(400, "Passenger(s) flight classes on the selected flights do not match"));
        //    }

        //    foreach (var flight in flights)
        //    {
        //        var orderedSeats = flight.Seats.OrderBy(s => s.Row).ThenBy(s => s.Letter).ToList();
        //        var emptySeats = orderedSeats.Where(s => s.SeatStatus == SeatStatusEnum.Empty).ToList();
        //        var seatsToAssign = new List<Seat>();

        //        var passengersWithoutSeats = passengers.Where(p => p.AssignedSeats.All(s => s.FlightId != flight.Id)).ToList();
        //        var passengersWithSeats = passengers.Except(passengersWithoutSeats).ToList();
        //        var groupSize = passengersWithoutSeats.Count;

        //        if (groupSize == 1)
        //        {
        //            var availableSeat = _SelectSeatForSinglePassenger(emptySeats);
        //            if (availableSeat != null)
        //            {
        //                seatsToAssign.Add(availableSeat);
        //            }
        //        }
        //        else
        //        {
        //            var potentialSeatGroups = _FindSeatsForGroup(orderedSeats, groupSize, false);
        //            seatsToAssign = _SelectSeatsForPassengerGroup(potentialSeatGroups, orderedSeats, groupSize);
        //        }

        //        for (int i = 0; i < groupSize; i++)
        //        {
        //            if (seatsToAssign[i] == null)
        //            {
        //                passengersWithoutSeats[i].Flights.FirstOrDefault(f => f.FlightId == flight.Id).AcceptanceStatus = AcceptanceStatusEnum.Standby;
        //            }
        //            passengersWithoutSeats[i].AssignedSeats.Add(seatsToAssign[i]);
        //        }

        //        var highestSequenceNumber = await _passengerFlightRepository.GetHighestSequenceNumberOfTheFlight(flight.Id);

        //        foreach (var passenger in passengers)
        //        {
        //            if (passenger.AssignedSeats.Any(s => s.FlightId == flight.Id))
        //            {
        //                passenger.Flights.FirstOrDefault(f => f.FlightId == flight.Id).AcceptanceStatus = AcceptanceStatusEnum.Accepted;
        //                passenger.Flights.FirstOrDefault(f => f.FlightId == flight.Id).BoardingSequenceNumber = ++highestSequenceNumber;
        //                passenger.Flights.FirstOrDefault(f => f.FlightId == flight.Id).BoardingZone = passenger.PriorityBoarding
        //                    ? BoardingZoneEnum.A : passenger.BaggageAllowance > 0
        //                    ? BoardingZoneEnum.B : BoardingZoneEnum.C;
        //            }
        //        }
        //    }

        //    await _passengerRepository.UpdateAsync(passengers.ToArray());

        //    return Ok();
        //}
    }
}