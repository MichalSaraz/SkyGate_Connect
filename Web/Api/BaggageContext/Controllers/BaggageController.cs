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

        /// <summary>
        /// Retrieves the details of a baggage by its tag number.
        /// </summary>
        /// <param name="tagNumber">The tag number of the baggage.</param>
        /// <returns>The details of the baggage with the specified tag number.</returns>
        /// <remarks>This method retrieves the baggage from the database based on the provided tag number.
        /// It includes related entities such as passenger, baggage tag, special bag, final destination, and flights.
        /// The baggage is retrieved with no tracking to improve performance.</remarks>
        [HttpGet("{tagNumber}/details")]
        public async Task<ActionResult<Baggage>> GetBaggageDetails(string tagNumber)
        {
            //ToDo: Add validation for tagNumber
            var baggage = await _baggageRepository.GetBaggageByTagNumber(tagNumber);

            var baggageDto = _mapper.Map<Baggage, BaggageDetailsDto>(baggage);

            return Ok(baggageDto);
        }

        /// <summary>
        /// Retrieves all bags for a given flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <returns>An asynchronous task that represents the operation. The task result contains an HTTP action result
        /// with the list of bags for the specified flight.</returns>
        [HttpGet("{flightId:int}/all-bags")]
        public async Task<ActionResult<List<Baggage>>> GetAllBags(int flightId)
        {
            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(b =>
                b.Flights.Any(fb => fb.FlightId == flightId));

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => 
                opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        /// <summary>
        /// Retrieves all bags of a specific special bag type for a given flight ID.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="specialBagType">The special bag type to filter by.</param>
        /// <returns>A list of baggage of the specified special bag type for the given flight ID.</returns>
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

        /// <summary>
        /// Retrieves all bags of a specific baggage type for a given flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="baggageType">The type of baggage to filter the bags by.</param>
        /// <returns>A list of bags filtered by the specified baggage type for the given flight.</returns>
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

        /// <summary>
        /// Retrieves all inactive bags for a specific flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains
        /// an <see cref="ActionResult"/> of type <see cref="List{Baggage}"/> that represents the inactive bags
        /// for the specified flight.</returns>
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

        /// <summary>
        /// Retrieves a list of all bags with onward connections for a given flight ID.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <returns>A list of bags with onward connections.</returns>
        [HttpGet("{flightId:int}/onward-connections")]
        public async Task<ActionResult<List<Baggage>>> GetAllBagsWithOnwardConnection(int flightId)
        {
            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId) && 
                c.Flights.Any(fb => fb.Flight.DepartureDateTime > c.Flights
                    .FirstOrDefault(b => b.FlightId == flightId).Flight.DepartureDateTime);

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageDetailsDto>>(bagList);

            return Ok(bagListDto);
        }       
    }
}

