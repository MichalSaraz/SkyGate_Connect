using System.Linq.Expressions;
using AutoMapper;
using Core.BaggageContext;
using Core.BaggageContext.Enums;
using Core.Dtos;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.BaggageContext.Controllers
{
    [ApiController]
    [Route("baggage")]
    public class BaggageController : ControllerBase
    {
        private readonly IBaggageRepository _baggageRepository;
        private readonly IMapper _mapper;

        public BaggageController(
            IBaggageRepository baggageRepository,
            IMapper mapper)
        {
            _baggageRepository = baggageRepository;
            _mapper = mapper;
        }

        // Get details of a bag
        [HttpGet("{tagNumber}/details")]
        public async Task<ActionResult<Baggage>> GetBaggageDetails(string tagNumber)
        {
            //ToDo: Add validation for tagNumber
            var baggage = await _baggageRepository.GetBaggageByTagNumber(tagNumber);

            var baggageDto = _mapper.Map<Baggage, BaggageDetailsDto>(baggage);

            return Ok(baggageDto);
        }

        // Get all bags for a flight
        [HttpGet("{flightId:int}/all-bags")]
        public async Task<ActionResult<List<Baggage>>> GetAllBags(int flightId)
        {
            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(b =>
                b.Flights.Any(fb => fb.FlightId == flightId));

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => 
                opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        // Get all bags for a flight by special bag type
        [HttpGet("{flightId:int}/special-bag-type/{specialBagType}")]
        public async Task<ActionResult<List<Baggage>>> GetAllBagsBySpecialBagType(int flightId,
            SpecialBagEnum specialBagType)
        {
            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId) && c.SpecialBag.SpecialBagType == specialBagType;

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => 
                opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        // Get all bags for a flight by baggage type
        [HttpGet("{flightId:int}/baggage-type/{baggageType}")]
        public async Task<ActionResult<List<Baggage>>> GetAllBagsByBaggageType(int flightId,
            BaggageTypeEnum baggageType)
        {
            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId && fb.BaggageType == baggageType);

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => 
                opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        // Get all bags for a flight with no baggage tag
        [HttpGet("{flightId:int}/inactive-bags")]
        public async Task<ActionResult<List<Baggage>>> GetAllInactiveBags(int flightId)
        {
            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId) && string.IsNullOrEmpty(c.BaggageTag.TagNumber);

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => 
                opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        // Get all bags for a flight with connection
        [HttpGet("{flightId:int}/onward-connections")]
        public async Task<ActionResult<List<Baggage>>> GetAllBagsWithOnwardConnection(int flightId)
        {
            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId && fb.Flight.DepartureDateTime > c.Flights
                    .FirstOrDefault(b => b.FlightId == flightId).Flight.DepartureDateTime);

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageDetailsDto>>(bagList);

            return Ok(bagListDto);
        }       
    }
}

