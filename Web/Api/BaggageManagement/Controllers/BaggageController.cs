using System.Linq.Expressions;
using AutoMapper;
using Core.BaggageContext;
using Core.BaggageContext.Enums;
using Core.Dtos;
using Core.FlightContext.JoinClasses;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Api.BaggageManagement.Models;
using Web.Errors;

namespace Web.Api.BaggageManagement.Controllers
{
    [ApiController]
    [Route("baggage-management")]
    public class BaggageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBaggageRepository _baggageRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IDestinationRepository _destinationRepository;

        public BaggageController(
            IMapper mapper, 
            IBaggageRepository baggageRepository,
            IPassengerRepository passengerRepository, 
            IFlightRepository flightRepository, 
            IDestinationRepository destinationRepository)
        {
            _mapper = mapper;
            _baggageRepository = baggageRepository;
            _passengerRepository = passengerRepository;
            _flightRepository = flightRepository;
            _destinationRepository = destinationRepository;
        }

        /// <summary>
        /// Retrieves the details of a baggage by its tag number.
        /// </summary>
        /// <param name="tagNumber">The tag number of the baggage.</param>
        /// <returns>The details of the baggage with the specified tag number.</returns>
        [HttpGet("tag-number/{tagNumber}/details")]
        public async Task<ActionResult<BaggageDetailsDto>> GetBaggageDetails(string tagNumber)
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
        [HttpGet("flight/{flightId:guid}/all-bags")]
        public async Task<ActionResult<List<BaggageOverviewDto>>> GetAllBags(Guid flightId)
        {
            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(b =>
                b.Flights.Any(fb => fb.FlightId == flightId));

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        /// <summary>
        /// Retrieves all bags of a specific special bag type for a given flight ID.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="specialBagType">The special bag type to filter by.</param>
        /// <returns>A list of baggage of the specified special bag type for the given flight ID.</returns>
        [HttpGet("flight/{flightId:guid}/special-bag-type/{specialBagType}")]
        public async Task<ActionResult<List<BaggageOverviewDto>>> GetAllBagsBySpecialBagType(Guid flightId,
            SpecialBagEnum specialBagType)
        {
            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId) && c.SpecialBag.SpecialBagType == specialBagType;

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        /// <summary>
        /// Retrieves all bags of a specific baggage type for a given flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="baggageType">The type of baggage to filter the bags by.</param>
        /// <returns>A list of bags filtered by the specified baggage type for the given flight.</returns>
        [HttpGet("flight/{flightId:guid}/baggage-type/{baggageType}")]
        public async Task<ActionResult<List<BaggageOverviewDto>>> GetAllBagsByBaggageType(Guid flightId,
            BaggageTypeEnum baggageType)
        {
            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId && fb.BaggageType == baggageType);

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        /// <summary>
        /// Retrieves all inactive bags for a specific flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result contains
        /// an <see cref="ActionResult"/> of type <see cref="List{Baggage}"/> that represents the inactive bags
        /// for the specified flight.</returns>
        [HttpGet("flight/{flightId:guid}/inactive-bags")]
        public async Task<ActionResult<List<BaggageOverviewDto>>> GetAllInactiveBags(Guid flightId)
        {
            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId) && string.IsNullOrEmpty(c.BaggageTag.TagNumber);

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        /// <summary>
        /// Retrieves a list of all bags with onward connections for a given flight ID.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <returns>A list of bags with onward connections.</returns>
        [HttpGet("flight/{flightId:guid}/onward-connections")]
        public async Task<ActionResult<List<BaggageDetailsDto>>> GetAllBagsWithOnwardConnection(Guid flightId)
        {
            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId) && c.Flights.Any(fb =>
                    fb.Flight.DepartureDateTime >
                    c.Flights.FirstOrDefault(b => b.FlightId == flightId).Flight.DepartureDateTime);

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);

            var bagListDto = _mapper.Map<List<BaggageDetailsDto>>(bagList);

            return Ok(bagListDto);
        }
        
        /// <summary>
        /// Adds baggage for a passenger on a specific flight.
        /// </summary>
        /// <param name="passengerId">The ID of the passenger.</param>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="addBaggageModels">The list of baggage models to add.</param>
        /// <returns>An ActionResult with the added baggage.</returns>
        [HttpPost("passenger/{passengerId:guid}/flight/{flightId:guid}/add-baggage")]
        public async Task<ActionResult<Baggage>> AddBaggage(Guid passengerId, Guid flightId,
            [FromBody] List<AddBaggageModel> addBaggageModels)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(passengerId);
            var selectedFlight = await _flightRepository.GetFlightByIdAsync(flightId);
            var destination = await _destinationRepository.GetDestinationByCriteriaAsync(d =>
                d.IATAAirportCode == addBaggageModels.FirstOrDefault().FinalDestination);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {passengerId} was not found."));
            }

            var baggageList = new List<Baggage>();

            foreach (var baggageModel in addBaggageModels)
            {
                var newBaggage = new Baggage(passenger.Id, destination.IATAAirportCode, baggageModel.Weight)
                {
                    SpecialBag = baggageModel.SpecialBagType.HasValue
                        ? new SpecialBag(baggageModel.SpecialBagType.Value, baggageModel.Description)
                        : null
                };

                switch (baggageModel.TagType)
                {
                    //ToDo: Add validation for tag number correct format
                    case TagTypeEnum.System when baggageModel.BaggageType == BaggageTypeEnum.Transfer:
                    case TagTypeEnum.Manual when !string.IsNullOrEmpty(baggageModel.TagNumber):
                        newBaggage.BaggageTag = new BaggageTag(baggageModel.TagNumber);
                        break;
                    case TagTypeEnum.System:
                        var number = _baggageRepository.GetNextSequenceValue("BaggageTagsSequence");
                        newBaggage.BaggageTag = new BaggageTag(selectedFlight.Airline, number);
                        break;
                }

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
            }

            await _baggageRepository.UpdateAsync(baggageList.ToArray());

            return Ok();
        }

        /// <summary>
        /// Edit baggage information for a passenger.
        /// </summary>
        /// <param name="passengerId">The ID of the passenger.</param>
        /// <param name="editBaggageModels">The list of baggage models containing the changes to apply.</param>
        /// <returns>The updated baggage information.</returns>
        [HttpPut("passenger/{passengerId:guid}/edit-baggage")]
        public async Task<ActionResult<Baggage>> EditBaggage(Guid passengerId,
            [FromBody] List<EditBaggageModel> editBaggageModels)
        {
            var changesToSave = new List<Baggage>();

            foreach (var model in editBaggageModels)
            {
                var selectedBaggage =
                    await _baggageRepository.GetBaggageByCriteriaAsync(b =>
                        b.Id == model.BaggageId && b.PassengerId == passengerId);

                if (selectedBaggage != null)
                {
                    selectedBaggage.Weight = model.Weight;

                    if (model.SpecialBagType.HasValue)
                    {
                        if (selectedBaggage.SpecialBag == null)
                        {
                            selectedBaggage.SpecialBag = new SpecialBag(model.SpecialBagType.Value, model.Description);
                        }
                        else
                        {
                            selectedBaggage.SpecialBag.SpecialBagType = model.SpecialBagType.Value;
                            selectedBaggage.SpecialBag.SpecialBagDescription = model.Description;
                        }
                    }
                    else if (!model.SpecialBagType.HasValue && selectedBaggage.SpecialBag != null)
                    {
                        selectedBaggage.SpecialBag = null;
                    }

                    changesToSave.Add(selectedBaggage);
                }
            }

            await _baggageRepository.UpdateAsync(changesToSave.ToArray());

            return Ok();
        }

        /// <summary>
        /// Deletes the selected baggage for a passenger.
        /// </summary>
        /// <param name="passengerId">The ID of the passenger.</param>
        /// <param name="baggageIds">The list of baggage IDs to delete.</param>
        /// <returns>Returns an ActionResult representing the HTTP response.</returns>
        [HttpDelete("passenger/{passengerId:guid}/delete-baggage")]
        public async Task<ActionResult> DeleteSelectedBaggage(Guid passengerId, [FromBody] List<Guid> baggageIds)
        {
            var selectedBaggage = new List<Baggage>();

            foreach (var baggageId in baggageIds)
            {
                var baggage =
                    await _baggageRepository.GetBaggageByCriteriaAsync(b =>
                        b.Id == baggageId && b.PassengerId == passengerId);

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
    }
}

