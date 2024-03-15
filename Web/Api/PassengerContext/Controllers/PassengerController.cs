using System.Linq.Expressions;
using AutoMapper;
using Core.BaggageContext;
using Core.BaggageContext.Enums;
using Core.Dtos;
using Core.FlightContext;
using Core.FlightContext.JoinClasses;
using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.JoinClasses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Web.Api.PassengerContext.Models;
using Web.Errors;

namespace Web.Api.PassengerContext.Controllers
{
    [ApiController]
    [Route("passenger")]
    public class PassengerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITimeProvider _timeProvider;
        private readonly IFlightRepository<BaseFlight> _flightRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IBaggageRepository _baggageRepository;
        private readonly IDestinationRepository _destinationRepository;

        public PassengerController(
            IMapper mapper,
            ITimeProvider timeProvider,
            IFlightRepository<BaseFlight> flightRepository, 
            IPassengerRepository passengerRepository, 
            IBaggageRepository baggageRepository, 
            IDestinationRepository destinationRepository)
        {
            _mapper = mapper;
            _timeProvider = timeProvider;
            _flightRepository = flightRepository;
            _passengerRepository = passengerRepository;
            _baggageRepository = baggageRepository;
            _destinationRepository = destinationRepository;
        }

        //
        [HttpPost("search")]
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
                (string.IsNullOrEmpty(model.PNR) || c.PNRId == model.PNR);

            var passengers = await _passengerRepository.GetPassengersByCriteriaAsync(criteria);

            var selectedFlight = await _flightRepository.GetFlightByCriteriaAsync<Flight>(f =>
                f.ScheduledFlightId.Substring(2) == model.FlightNumber && f.AirlineId == model.AirlineId &&
                f.DepartureDateTime.Date == model.DepartureDate.Value.Date);

            var passengersDto = _mapper.Map<List<PassengerOverviewDto>>(passengers, opt =>
            {
                opt.Items["DepartureDateTime"] = selectedFlight.DepartureDateTime;
                opt.Items["FlightId"] = selectedFlight.Id;
            });

            return Ok(passengersDto);
        }

        [HttpGet("{id:guid}/flight/{flightId:int}/details")]
        public async Task<ActionResult<Passenger>> GetPassengerDetails(Guid id, int flightId)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id, false, true);
            var selectedFlight = await _flightRepository.GetFlightByIdAsync<Flight>(flightId, false);

            var passengerDto = _mapper.Map<PassengerDetailsDto>(passenger, opt =>
            {
                opt.Items["DepartureDateTime"] = selectedFlight.DepartureDateTime;
                opt.Items["FlightId"] = flightId;
            });

            if (passengerDto == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger {id} not found"));
            }

            return Ok(passengerDto);
        }

        [HttpPost("{id:guid}/flight/{flightId:int}/add-baggage")]
        public async Task<ActionResult<Baggage>> AddBaggage(Guid id, int flightId,
            [FromBody] List<AddBaggageModel> addBaggageModels)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id);
            var selectedFlight = await _flightRepository.GetFlightByIdAsync<Flight>(flightId);
            var destination = await _destinationRepository.GetDestinationByCriteriaAsync(d =>
                d.IATAAirportCode == addBaggageModels.FirstOrDefault().FinalDestination);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            var baggageList = new List<Baggage>();

            foreach (var baggageModel in addBaggageModels)
            {
                var newBaggage = new Baggage(passenger.Id, destination.IATAAirportCode, baggageModel.Weight)
                {
                    SpecialBag =
                        baggageModel.SpecialBagType.HasValue
                            ? new SpecialBag(baggageModel.SpecialBagType.Value, baggageModel.Description)
                            : null,
                    BaggageTag = CreateBaggageTag()
                };

                await _baggageRepository.AddAsync(newBaggage);

                baggageList.Add(newBaggage);

                var orderedFlights = passenger.Flights
                    .Where(pf => pf.Flight.DepartureDateTime >= selectedFlight.DepartureDateTime)
                    .OrderBy(pf => pf.Flight.DepartureDateTime)
                    .ToList();

                var isFirstIteration = true;

                foreach (var connectingFlight in orderedFlights)
                {
                    var baggageType = isFirstIteration && baggageModel.BaggageType == BaggageTypeEnum.Local
                        ? BaggageTypeEnum.Local
                        : BaggageTypeEnum.Transfer;

                    var flightBaggage = new FlightBaggage(connectingFlight.Flight.Id, newBaggage.Id, baggageType);

                    newBaggage.Flights.Add(flightBaggage);

                    if (connectingFlight.Flight.DestinationToId == baggageModel.FinalDestination)
                    {
                        break;
                    }

                    isFirstIteration = false;
                }

                continue;

                BaggageTag CreateBaggageTag()
                {
                    switch (baggageModel.TagType)
                    {
                        //ToDo: Add validation for tag number correct format
                        case TagTypeEnum.System when baggageModel.BaggageType == BaggageTypeEnum.Transfer:
                        case TagTypeEnum.Manual when !string.IsNullOrEmpty(baggageModel.TagNumber):
                            return new BaggageTag(baggageModel.TagNumber);
                        case TagTypeEnum.System:
                            return new BaggageTag(selectedFlight.Airline,
                                _baggageRepository.GetNextSequenceValueAsync("BaggageTagsSequence").Result);
                        default:
                            return null;
                    }
                }
            }

            await _baggageRepository.UpdateAsync(baggageList.ToArray());

            return Ok();
        }

        [HttpPut("{id:guid}/edit-baggage")]
        public async Task<ActionResult<Baggage>> EditBaggage(Guid id,
            [FromBody] List<EditBaggageModel> editBaggageModels)
        {
            var changesToSave = new List<Baggage>();

            foreach (var editBaggageModel in editBaggageModels)
            {
                var selectedBaggage =
                    await _baggageRepository.GetBaggageByCriteriaAsync(b =>
                        b.Id == editBaggageModel.BaggageId && b.PassengerId == id);

                if (selectedBaggage != null)
                {
                    selectedBaggage.Weight = editBaggageModel.Weight;

                    if (editBaggageModel.SpecialBagType.HasValue)
                    {
                        if (selectedBaggage.SpecialBag == null)
                        {
                            selectedBaggage.SpecialBag = new SpecialBag(editBaggageModel.SpecialBagType.Value,
                                editBaggageModel.Description);
                        }
                        else
                        {
                            selectedBaggage.SpecialBag.SpecialBagType = editBaggageModel.SpecialBagType.Value;
                            selectedBaggage.SpecialBag.SpecialBagDescription = editBaggageModel.Description;
                        }
                    }
                    else if (!editBaggageModel.SpecialBagType.HasValue && selectedBaggage.SpecialBag != null)
                    {
                        selectedBaggage.SpecialBag = null;
                    }

                    changesToSave.Add(selectedBaggage);
                }
            }

            if (!changesToSave.Any())
            {
                return Ok(Array.Empty<Baggage>());
            }

            var updatedBaggage = await _baggageRepository.UpdateAsync(changesToSave.ToArray());

            return Ok(updatedBaggage);
        }

        [HttpDelete("{id:guid}/delete-baggage")]
        public async Task<ActionResult> DeleteSelectedBaggage(Guid id, [FromBody] List<Guid> baggageIds)
        {
            var selectedBaggage = new List<Baggage>();

            foreach (var baggageId in baggageIds)
            {
                var baggage =
                    await _baggageRepository.GetBaggageByCriteriaAsync(b => b.Id == baggageId && b.PassengerId == id);

                if (baggage != null)
                {
                    selectedBaggage.Add(baggage);
                }
            }

            if (selectedBaggage.Count != baggageIds.Count)
            {
                return BadRequest(new ApiResponse(400, "Invalid baggage IDs."));
            }

            await _baggageRepository.DeleteAsync(selectedBaggage.ToArray());

            return NoContent();
        }

        [HttpGet("{id:guid}/all-bags")]
        public async Task<ActionResult<List<Baggage>>> GetAllPassengersBags(Guid id)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id, false, true);

            var passengerDto = _mapper.Map<List<BaggageDetailsDto>>(passenger.PassengerCheckedBags);

            return Ok(passengerDto);
        }

        [HttpPost("{id:guid}/flight/{flightId:int}/add-connecting-flight")]
        public async Task<ActionResult<BaseFlight>> AddConnectingFlight(Guid id, int flightId, bool isInbound,
            [FromBody] List<AddConnectingFlightModel> addConnectingFlightModels)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id);
            var currentPassengerFlights = passenger.Flights.Select(pf => pf.Flight).ToList();
            var currentFlight = await _flightRepository.GetFlightByIdAsync<Flight>(flightId, false);

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

                var newPassengerFlight = new PassengerFlight(passenger.Id, currentPassengerFlights.Last().Id,
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

            var otherFlight = await _flightRepository.GetFlightByCriteriaAsync(otherFlightCriteria, true);
            if (otherFlight != null) return otherFlight;

            otherFlight = new OtherFlight(connectingFlightModel.FlightNumber, parsedDepartureDateTime, null,
                connectingFlightModel.DestinationFrom, connectingFlightModel.DestinationTo,
                connectingFlightModel.AirlineId);

            await _flightRepository.AddAsync(otherFlight);
            return otherFlight;
        }

        private Expression<Func<Flight, bool>> _BuildFlightCriteria(AddConnectingFlightModel connectingFlightModel,
            DateTime parsedDepartureDateTime)
        {
            return f => f.AirlineId == connectingFlightModel.AirlineId &&
                        f.ScheduledFlightId.Substring(2) == connectingFlightModel.FlightNumber &&
                        f.DepartureDateTime.Date == parsedDepartureDateTime.Date &&
                        f.DestinationFromId == connectingFlightModel.DestinationFrom &&
                        f.DestinationToId == connectingFlightModel.DestinationTo;
        }

        private Expression<Func<OtherFlight, bool>> _BuildOtherFlightCriteria(
            AddConnectingFlightModel connectingFlightModel, DateTime parsedDepartureDateTime)
        {
            return of => of.AirlineId == connectingFlightModel.AirlineId &&
                         of.FlightNumber == connectingFlightModel.FlightNumber &&
                         of.DepartureDateTime.Date == parsedDepartureDateTime.Date &&
                         of.DestinationFromId == connectingFlightModel.DestinationFrom &&
                         of.DestinationToId == connectingFlightModel.DestinationTo;
        }

        [HttpDelete("{id:guid}/delete-connecting-flight")]
        public async Task<ActionResult<BaseFlight>> DeleteConnectingFlight(Guid id, [FromBody] List<int> flightIds)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id);

            foreach (var flightId in flightIds)
            {
                var flight = passenger.Flights.FirstOrDefault(pf => pf.Flight.Id == flightId);

                if (flight != null)
                {
                    passenger.Flights.Remove(flight);
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Invalid flight IDs."));
                }
            }

            await _passengerRepository.UpdateAsync(passenger);

            return NoContent();
        }

        //[HttpPost("{id}/add-special-service-request")]
    }
}