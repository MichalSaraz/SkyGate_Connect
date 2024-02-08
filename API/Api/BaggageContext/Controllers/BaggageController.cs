using API.Errors;
using Core.BaggageContext;
using Core.BaggageContext.Enums;
using Core.FlightContext;
using Core.FlightContext.FlightInfo;
using Core.FlightContext.JoinClasses;
using Core.Interfaces;
using Core.PassengerContext;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Api.BaggageContext.Controllers
{
    [ApiController]
    [Route("baggage")]
    public class BaggageController : ControllerBase
    {
        private readonly IBaggageRepository _baggageRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IFlightRepository _flightRepository;

        public BaggageController(
            IBaggageRepository baggageRepository,
            IPassengerRepository passengerRepository,
            IFlightRepository flightRepository
            )
        {
            _baggageRepository = baggageRepository;
            _passengerRepository = passengerRepository;
            _flightRepository = flightRepository;           
        }
        // Get details of a bag
        [HttpGet("{tagNumber}/details")]
        public async Task<ActionResult<Baggage>> GetBaggageDetails(string tagNumber)
        {
            var baggage = await _baggageRepository.GetBaggageByCriteriaAsync(f =>
                f.BaggageTag.TagNumber == tagNumber);

            return Ok(baggage);
        }

        // Get all bags for a flight
        [HttpGet("{flightId}/all-bags")]
        public async Task<ActionResult<List<Baggage>>> GetAllBags(int flightId)
        {
            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(f =>
                f.Flights.Any(_ => _.FlightId == flightId));

            return Ok(bagList);
        }

        // Get all bags for a flight by special bag type
        [HttpGet("{flightId}/{specialBagType}")]
        public async Task<ActionResult<List<Baggage>>> GetAllBagsBySpecialBagType(int flightId,
            SpecialBagEnum specialBagType)
        {
            Expression<Func<Baggage, bool>> criteria = f =>
                f.Flights.Any(_ => _.FlightId == flightId) &&
                f.SpecialBag.SpecialBagType == specialBagType;

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            return Ok(bagList);
        }

        // Get all bags for a flight by baggage type
        [HttpGet("{flightId}/{baggageType}")]
        public async Task<ActionResult<List<Baggage>>> GetAllBagsByBaggageType(int flightId,
            BaggageTypeEnum baggageType)
        {
            Expression<Func<Baggage, bool>> criteria = f =>
                f.Flights.Any(_ => _.FlightId == flightId && _.BaggageType == baggageType);

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            return Ok(bagList);
        }

        // Get all bags for a flight with no baggage tag
        [HttpGet("{flightId}/inactive-bags")]
        public async Task<ActionResult<List<Baggage>>> GetAllInactiveBags(int flightId)
        {
            Expression<Func<Baggage, bool>> criteria = f =>
                f.Flights.Any(_ => _.FlightId == flightId) &&
                f.BaggageTag == null;

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            return Ok(bagList);
        }

        // Get all bags for a flight with connection
        [HttpGet("{flightId}/onward-connections")]
        public async Task<ActionResult<List<Baggage>>> GetAllBagsWithOnwardConnection(int flightId)
        {
            Expression<Func<Baggage, bool>> criteria = f =>
                f.Flights.Any(_ => _.FlightId == flightId) &&
                f.Flights.Any(_ => _.Flight.DepartureDateTime > f.Flights
                    .FirstOrDefault(_ => _.FlightId == flightId).Flight.DepartureDateTime);

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            return Ok(bagList);
        }

        // Get all bags of a passenger
        [HttpGet("passenger/{passengerId}")]
        public async Task<ActionResult<List<Baggage>>> GetAllPassengersBags(Guid passengerId)
        {
            var passenger = await _passengerRepository.GetPassengerByCriteriaAsync(f =>
                f.Id == passengerId);

            return Ok(passenger.PassengerCheckedBags);
        }        
    }
}

