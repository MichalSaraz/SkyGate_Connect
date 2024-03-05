using AutoMapper;
using Core.BaggageContext;
using Core.BaggageContext.Enums;
using Core.Dtos;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Api.BaggageContext.Controllers
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
        [HttpGet("{flightId}/all-bags")]
        public async Task<ActionResult<List<Baggage>>> GetAllBags(int flightId)
        {
            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(f =>
                f.Flights.Any(_ => _.FlightId == flightId));

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => 
                opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        // Get all bags for a flight by special bag type
        [HttpGet("{flightId}/special-bag-type/{specialBagType}")]
        public async Task<ActionResult<List<Baggage>>> GetAllBagsBySpecialBagType(int flightId,
            SpecialBagEnum specialBagType)
        {
            Expression<Func<Baggage, bool>> criteria = f =>
                f.Flights.Any(_ => _.FlightId == flightId) &&
                f.SpecialBag.SpecialBagType == specialBagType;

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => 
                opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        // Get all bags for a flight by baggage type
        [HttpGet("{flightId}/baggage-type/{baggageType}")]
        public async Task<ActionResult<List<Baggage>>> GetAllBagsByBaggageType(int flightId,
            BaggageTypeEnum baggageType)
        {
            Expression<Func<Baggage, bool>> criteria = f =>
                f.Flights.Any(_ => _.FlightId == flightId && _.BaggageType == baggageType);

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => 
                opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        // Get all bags for a flight with no baggage tag
        [HttpGet("{flightId}/inactive-bags")]
        public async Task<ActionResult<List<Baggage>>> GetAllInactiveBags(int flightId)
        {
            Expression<Func<Baggage, bool>> criteria = f =>
                f.Flights.Any(_ => _.FlightId == flightId) && string.IsNullOrEmpty(f.BaggageTag.TagNumber);

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => 
                opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        // Get all bags for a flight with connection
        [HttpGet("{flightId}/onward-connections")]
        public async Task<ActionResult<List<Baggage>>> GetAllBagsWithOnwardConnection(int flightId)
        {
            Expression<Func<Baggage, bool>> criteria = f =>
                f.Flights.Any(_ => _.FlightId == flightId) &&
                f.Flights.Any(_ => _.Flight.DepartureDateTime > f.Flights
                    .FirstOrDefault(_ => _.FlightId == flightId).Flight.DepartureDateTime);
            //|| (f.Flights.Any(_ => _.Flight.DepartureDateTime.TimeOfDay == TimeSpan.Zero));

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageDetailsDto>>(bagList);

            return Ok(bagListDto);
        }       
    }
}

