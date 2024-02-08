using API.Errors;
using AutoMapper;
using Core.BaggageContext.Enums;
using Core.BaggageContext;
using Core.FlightContext.JoinClasses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.Api.FlightContext.Models;
using Core.Dtos;
using Core.FlightContext;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using Core.PassengerContext;
using API.Api.PassengerContext.Models;
using System.Diagnostics;

namespace API.Api.PassengerContext.Controllers
{
    [ApiController]
    [Route("passenger")]
    public class PassengerController : ControllerBase
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IBaggageRepository _baggageRepository;
        private readonly IDestinationRepository _destinationRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<PassengerController> _logger;

        public PassengerController(
            IFlightRepository flightRepository,
            ITimeProvider timeProvider,
            IPassengerRepository passengerRepository,
            IBaggageRepository baggageRepository,
            IMapper mapper,
            IDestinationRepository destinationRepository,
            ILogger<PassengerController> logger)
        {
            _flightRepository = flightRepository;
            _timeProvider = timeProvider;
            _passengerRepository = passengerRepository;
            _baggageRepository = baggageRepository;
            _mapper = mapper;
            _destinationRepository = destinationRepository;
            _logger = logger;
        }

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

            if (string.IsNullOrEmpty(model.FlightNumber) ||
                string.IsNullOrEmpty(model.AirlineId) ||
                !model.DepartureDate.HasValue ||
                string.IsNullOrEmpty(model.DocumentNumber) &&
                string.IsNullOrEmpty(model.LastName) &&
                string.IsNullOrEmpty(model.PNR) &&
                string.IsNullOrEmpty(model.DestinationFrom) &&
                string.IsNullOrEmpty(model.DestinationTo) &&
                string.IsNullOrEmpty(model.SeatNumber))
            {
                return BadRequest(
                    new ApiResponse(
                        400, "All mandatory plus one optional field must be filled in for the search criteria."));
            }

            Expression<Func<Passenger, bool>> criteria = f =>
                f.Flights.Any(g =>
                    g.Flight.ScheduledFlightId.Substring(2) == model.FlightNumber) &&
                f.Flights.Any(g =>
                    g.Flight.ScheduledFlight.AirlineId == model.AirlineId) &&
                f.Flights.Any(g =>
                    g.Flight.DepartureDateTime.Date == model.DepartureDate.Value.Date) &&
                (
                    f.TravelDocuments.Any(g => g.DocumentNumber == model.DocumentNumber) ||
                    string.IsNullOrEmpty(model.DocumentNumber)) &&
                (
                    f.Flights.Any(g => g.Flight.ScheduledFlight.DestinationFromId == model.DestinationFrom) ||
                    string.IsNullOrEmpty(model.DestinationFrom)) &&
                (
                    f.Flights.Any(g => g.Flight.ScheduledFlight.DestinationToId == model.DestinationTo) ||
                    string.IsNullOrEmpty(model.DestinationTo)) &&
                (
                    f.AssignedSeats.Any(g => g.SeatNumber == model.SeatNumber) ||
                    string.IsNullOrEmpty(model.SeatNumber)) &&
                (
                    f.LastName.Substring(0, model.LastName.Length) == model.LastName ||
                    string.IsNullOrEmpty(model.LastName)) &&
                (
                    f.PNRId == model.PNR ||
                    string.IsNullOrEmpty(model.PNR));

            var selectedFlight = await _flightRepository.GetFlightByCriteriaAsync(f =>
                f.ScheduledFlightId.Substring(2) == model.FlightNumber &&
                f.ScheduledFlight.AirlineId == model.AirlineId &&
                f.DepartureDateTime.Date == model.DepartureDate.Value.Date);

            var passengers = await _passengerRepository.GetPassengersByCriteriaAsync(criteria);

            var passengersDto = _mapper.Map<List<PassengerOverviewDto>>(passengers);

            foreach (var passengerDto in passengersDto)
            {
                passengerDto.Flights = passengerDto.Flights
                    .Where(flt => flt.FlightId == selectedFlight.Id)
                    .ToList();

                passengerDto.AssignedSeats = passengerDto.AssignedSeats
                    .Where(seat => seat.FlightId == selectedFlight.Id)
                    .ToList();
            }

            return Ok(passengersDto);
        }

        [HttpGet("{id}/flight/{flightId}/details")]
        public async Task<ActionResult<Passenger>> GetPassengerDetails(Guid id, int flightId)
        {
            var passenger = await _passengerRepository.GetPassengerByCriteriaAsync(f => f.Id == id);
            var selectedFlight = await _flightRepository.GetFlightByCriteriaAsync(f => f.Id == flightId);

            var passengerDto = _mapper.Map<PassengerDetailsDto>(passenger); 

            if (passengerDto == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger {id} not found"));
            }

            var connectingFlights = passenger.Flights
                .Where(f => f.Flight.DepartureDateTime > selectedFlight.DepartureDateTime)
                .ToList();

            var inboundFlights = passenger.Flights
                .Where(f => f.Flight.DepartureDateTime < selectedFlight.DepartureDateTime)
                .ToList();

            passengerDto.ConnectingFlights = _mapper.Map<List<PassengerFlightDto>>(connectingFlights);
            passengerDto.InboundFlights = _mapper.Map<List<PassengerFlightDto>>(inboundFlights);

            return Ok(passengerDto);
        }

        [HttpPut("{id}/flight/{flightId}/edit-baggage")]
        public async Task<ActionResult<Baggage>> EditBaggage(Guid id, int flightId,
            [FromBody] List<EditBaggageModel> editBaggageModels)
        {
            var passenger = await _passengerRepository.GetPassengerByCriteriaAsync(f =>
                f.Id == id);

            var baggage = await _baggageRepository.GetBaggageByCriteriaAsync(f =>
                    f.Id == editBaggageModels.FirstOrDefault().BaggageId);

            if (passenger == null)
            {
                return NotFound($"Passenger with Id {id} was not found.");
            }

            var changesToSave = new List<Baggage>();

            foreach (var editBaggageModel in editBaggageModels)
            {
                if (baggage != null)
                {
                    baggage.Weight = editBaggageModel.Weight;

                    if (editBaggageModel.SpecialBagType.HasValue)
                    {
                        if (baggage.SpecialBag == null)
                        {
                            baggage.SpecialBag = new SpecialBag(editBaggageModel.SpecialBagType.Value,
                                editBaggageModel.Description);
                        }
                        else
                        {
                            baggage.SpecialBag.SpecialBagType = editBaggageModel.SpecialBagType.Value;
                            baggage.SpecialBag.SpecialBagDescription = editBaggageModel.Description;
                        }
                    }
                    else if (!editBaggageModel.SpecialBagType.HasValue && baggage.SpecialBag != null)
                    {
                        baggage.SpecialBag = null;
                    }
                }
                changesToSave.Add(baggage);
            }
            await _baggageRepository.UpdateAsync(changesToSave.ToArray());

            return Ok();
        }
        
        [HttpPost("{id}/flight/{flightId}/add-baggage")]
        public async Task<ActionResult<Baggage>> AddBaggage(Guid id, int flightId,
            [FromBody] List<AddBaggageModel> addBaggageModels)
        {
            var passenger = await _passengerRepository.GetPassengerByCriteriaAsync(f => f.Id == id);
            var selectedFlight = await _flightRepository.GetFlightByCriteriaAsync(f => f.Id == flightId);
            var destination = await _destinationRepository.GetDestinationByCriteriaAsync(f =>
                f.IATAAirportCode == addBaggageModels.FirstOrDefault().FinalDestination);

            if (passenger == null)
            {
                return NotFound($"Passenger with Id {id} was not found.");
            }

            var baggageList = new List<Baggage>();

            foreach (var baggageModel in addBaggageModels)
            {
                var newBaggage = new Baggage
                {
                    Passenger = passenger,
                    Weight = baggageModel.Weight,
                    FinalDestination = destination
                };

                if (baggageModel.SpecialBagType.HasValue)
                {
                    newBaggage.SpecialBag = new SpecialBag(baggageModel.SpecialBagType.Value, baggageModel.Description);
                }

                //ToDo: Add validation for tag number correct format
                if ((baggageModel.TagType == TagTypeEnum.System && baggageModel.BaggageType == BaggageTypeEnum.Transfer) || 
                     baggageModel.TagType == TagTypeEnum.Manual && !string.IsNullOrEmpty(baggageModel.TagNumber))
                {
                    newBaggage.BaggageTag = new BaggageTag(baggageModel.TagNumber);
                }
                else if (baggageModel.TagType == TagTypeEnum.System)
                {
                    var number = _baggageRepository.GetNextSequenceValue("BaggageTagsSequence");
                    var baggageTag = new BaggageTag(passenger.Flights.FirstOrDefault()?.Flight.ScheduledFlight.Airline, number);
                    newBaggage.BaggageTag = baggageTag;
                }

                await _baggageRepository.AddAsync(newBaggage);
                baggageList.Add(newBaggage);

                var orderedFlights = passenger.Flights
                    .Where(f => f.Flight.DepartureDateTime >= selectedFlight.DepartureDateTime)
                    .OrderBy(f => f.Flight.DepartureDateTime)
                    .ToList();

                bool isFirstIteration = true;

                foreach (var connectingFlight in orderedFlights)
                {
                    var baggageType = isFirstIteration && baggageModel.BaggageType == BaggageTypeEnum.Local
                        ? BaggageTypeEnum.Local
                        : BaggageTypeEnum.Transfer;

                    FlightBaggage flightBaggage = new FlightBaggage
                    {
                        Flight = connectingFlight.Flight,
                        Baggage = newBaggage,
                        BaggageType = baggageType
                    };

                    newBaggage.Flights.Add(flightBaggage);

                    if (connectingFlight.Flight.ScheduledFlight.DestinationToId == baggageModel.FinalDestination)
                    {
                        break;
                    }

                    isFirstIteration = false;
                }
            }

            await _baggageRepository.UpdateAsync(baggageList.ToArray());

            return Ok();
        }

        [HttpDelete("{id}/delete-baggage")]
        public async Task<ActionResult> DeleteSelectedBaggage(Guid id, int flightId,
            [FromBody] List<Guid> baggageIds)
        {
            var passenger = await _passengerRepository.GetPassengerByCriteriaAsync(f => f.Id == id);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            var selectedBaggage = passenger.PassengerCheckedBags.Where(b => baggageIds.Contains(b.Id)).ToList();

            if (selectedBaggage.Count != baggageIds.Count)
            {
                return BadRequest(new ApiResponse(400, "Invalid baggage IDs."));
            }

            await _baggageRepository.DeleteAsync(selectedBaggage.ToArray());

            return NoContent();
        }
    }    
}
