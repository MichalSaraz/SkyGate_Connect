using System.Linq.Expressions;
using AutoMapper;
using Core.BaggageContext;
using Core.Dtos;
using Core.FlightContext;
using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Core.PassengerContext.JoinClasses;
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

        public PassengerController(
            IMapper mapper,
            ITimeProvider timeProvider,
            IFlightRepository flightRepository,
            IPassengerRepository passengerRepository,
            IInfantRepository infantRepository,
            IPassengerBookingDetailsRepository passengerBookingDetailsRepository)
        {
            _mapper = mapper;
            _timeProvider = timeProvider;
            _flightRepository = flightRepository;
            _passengerRepository = passengerRepository;
            _infantRepository = infantRepository;
            _passengerBookingDetailsRepository = passengerBookingDetailsRepository;
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
        /// Adds a non-recurring passenger to a selected flight.
        /// </summary>
        /// <param name="flightId">The ID of the selected flight.</param>
        /// <param name="model">The model containing the details of the non-recurring passenger.</param>
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
    }
}