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
        /// <returns>An <see cref="ActionResult{T}"/> containing the <see cref="BaggageDetailsDto"/> object.</returns>
        [HttpGet("tag-number/{tagNumber}/details")]
        public async Task<ActionResult<BaggageDetailsDto>> GetBaggageDetails(string tagNumber)
        {
            //ToDo: Add validation for tagNumber
            var baggage = await _baggageRepository.GetBaggageByTagNumber(tagNumber);
            
            if (baggage == null)
            {
                return NotFound(new ApiResponse(404, $"Baggage with tag number {tagNumber} was not found."));
            }

            var baggageDto = _mapper.Map<Baggage, BaggageDetailsDto>(baggage);

            return Ok(baggageDto);
        }

        /// <summary>
        /// Retrieves all bags for a given flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="BaggageOverviewDto"/> objects.</returns>
        [HttpGet("flight/{flightId:guid}/all-bags")]
        public async Task<ActionResult<List<BaggageOverviewDto>>> GetAllBags(Guid flightId)
        {
            if (!await _flightRepository.ExistsAsync(flightId))
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} does not exist"));
            }

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(b =>
                b.Flights.Any(fb => fb.FlightId == flightId));
            
            if (bagList.Count == 0)
            {
                return Ok(new ApiResponse(200, "No bags found for this flight."));
            }

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        /// <summary>
        /// Retrieves all bags of a specific special bag type for a given flight ID.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="specialBagType">The special bag type to filter by.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="BaggageOverviewDto"/> objects.</returns>
        [HttpGet("flight/{flightId:guid}/special-bag-type/{specialBagType}")]
        public async Task<ActionResult<List<BaggageOverviewDto>>> GetAllBagsBySpecialBagType(Guid flightId,
            SpecialBagEnum specialBagType)
        {
            if (!await _flightRepository.ExistsAsync(flightId))
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} does not exist"));
            }

            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId) && c.SpecialBag != null &&
                c.SpecialBag.SpecialBagType == specialBagType;

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);
            
            if (bagList.Count == 0)
            {
                return Ok(new ApiResponse(200, $"No {specialBagType} found for this flight."));
            }

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        /// <summary>
        /// Retrieves all bags of a specific baggage type for a given flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="baggageType">The type of baggage to filter the bags by.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="BaggageOverviewDto"/> objects.</returns>
        [HttpGet("flight/{flightId:guid}/baggage-type/{baggageType}")]
        public async Task<ActionResult<List<BaggageOverviewDto>>> GetAllBagsByBaggageType(Guid flightId,
            BaggageTypeEnum baggageType)
        {
            if (!await _flightRepository.ExistsAsync(flightId))
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} does not exist"));
            }

            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId && fb.BaggageType == baggageType);

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);
            
            if (bagList.Count == 0)
            {
                return Ok(new ApiResponse(200, $"No bags {baggageType} bags found for this flight."));
            }

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        /// <summary>
        /// Retrieves a list of all inactive bags for a specified flight.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="BaggageOverviewDto"/> objects.</returns>
        [HttpGet("flight/{flightId:guid}/inactive-bags")]
        public async Task<ActionResult<List<BaggageOverviewDto>>> GetAllInactiveBags(Guid flightId)
        {
            if (!await _flightRepository.ExistsAsync(flightId))
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} does not exist"));
            }

            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId) && (c.BaggageTag == null ||
                string.IsNullOrEmpty(c.BaggageTag.TagNumber));

            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);
            
            if (bagList.Count == 0)
            {
                return Ok(new ApiResponse(200, "No inactive bags found for this flight."));
            }

            var bagListDto = _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => opt.Items["FlightId"] = flightId);

            return Ok(bagListDto);
        }

        /// <summary>
        /// Retrieves a list of all bags with onward connections for a given flight ID.
        /// </summary>
        /// <param name="flightId">The ID of the flight.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="BaggageDetailsDto"/> objects.</returns>
        [HttpGet("flight/{flightId:guid}/onward-connections")]
        public async Task<ActionResult<List<BaggageDetailsDto>>> GetAllBagsWithOnwardConnection(Guid flightId)
        {
            if (!await _flightRepository.ExistsAsync(flightId))
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} does not exist"));
            }

            Expression<Func<Baggage, bool>> criteria = c =>
                c.Flights.Any(fb => fb.FlightId == flightId) && c.Flights
                    .Where(fb => fb.FlightId == flightId && fb.Flight != null)
                    .Any(fb => c.Flights.Any(fb2 =>
                        fb2.Flight != null && fb2.Flight.DepartureDateTime > fb.Flight.DepartureDateTime));
            
            var bagList = await _baggageRepository.GetAllBaggageByCriteriaAsync(criteria);
            
            if (bagList.Count == 0)
            {
                return Ok(new ApiResponse(200, $"No bags found with onward connections for this flight."));
            }

            var bagListDto = _mapper.Map<List<BaggageDetailsDto>>(bagList);

            return Ok(bagListDto);
        }
        
        /// <summary>
        /// Adds baggage for a passenger on a specific flight.
        /// </summary>
        /// <param name="passengerId">The ID of the passenger.</param>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="addBaggageModels">The list of baggage models to add.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="BaggageOverviewDto"/> objects representing the added baggage.</returns>
        [HttpPost("passenger/{passengerId:guid}/flight/{flightId:guid}/add-baggage")]
        public async Task<ActionResult<BaggageOverviewDto>> AddBaggage(Guid passengerId, Guid flightId,
            [FromBody] List<AddBaggageModel> addBaggageModels)
        {
            if (addBaggageModels.Count == 0) {
                return BadRequest(new ApiResponse(400, "No baggage data provided."));
            }

            if (addBaggageModels.Any(bm => bm.TagType == TagTypeEnum.Manual && string.IsNullOrEmpty(bm.TagNumber)))
            {
                return BadRequest(new ApiResponse(400, "All baggage with TagType 'Manual' must have a TagNumber."));
            }

            var passenger = await _passengerRepository.GetPassengerByIdAsync(passengerId, false, true);
            var selectedFlight = await _flightRepository.GetFlightByIdAsync(flightId);
            var destination = await _destinationRepository.GetDestinationByCriteriaAsync(d =>
                d.IATAAirportCode == addBaggageModels.First().FinalDestination);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {passengerId} was not found."));
            }
            
            if (selectedFlight == null)
            {
                return NotFound(new ApiResponse(404, $"Flight with Id {flightId} was not found."));
            }
            
            if (destination == null)
            {
                return NotFound(new ApiResponse(404,
                    $"Destination with IATA Airport Code {addBaggageModels.First().FinalDestination} was not found."));
            }

            if (passenger.PassengerCheckedBags.Any(b =>
                    addBaggageModels.Where(a => a.FinalDestination != b.DestinationId).ToList().Count > 0))
            {
                return BadRequest(new ApiResponse(400, "All baggage must have the same final destination."));
            }

            var bagList = new List<Baggage>();

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

                var orderedFlights = passenger.Flights
                    .Where(pf => pf.Flight != null && pf.Flight.DepartureDateTime >= selectedFlight.DepartureDateTime)
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

                bagList.Add(newBaggage);
            }

            await _baggageRepository.UpdateAsync(bagList.ToArray());

            var addedBaggageList =
                _mapper.Map<List<BaggageOverviewDto>>(bagList, opt => opt.Items["FlightId"] = flightId);

            foreach (var baggageDetails in addedBaggageList)
            {
                baggageDetails.PassengerFirstName = passenger.FirstName;
                baggageDetails.PassengerLastName = passenger.LastName;
            }

            return Ok(addedBaggageList);
        }

        /// <summary>
        /// Edit baggage information for a passenger.
        /// </summary>
        /// <param name="passengerId">The ID of the passenger.</param>
        /// <param name="editBaggageModels">The list of baggage models containing the changes to apply.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="BaggageOverviewDto"/> objects representing the updated baggage.</returns>   
        [HttpPut("passenger/{passengerId:guid}/edit-baggage")]
        public async Task<ActionResult<BaggageOverviewDto>> EditBaggage(Guid passengerId,
            [FromBody] List<EditBaggageModel> editBaggageModels)
        {
            var changesToSave = new List<Baggage>();

            if (!await _passengerRepository.ExistsAsync(passengerId))
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {passengerId} does not exist."));
            }

            foreach (var model in editBaggageModels)
            {
                var selectedBaggage =
                    await _baggageRepository.GetBaggageByCriteriaAsync(b =>
                        b.Id == model.BaggageId && b.PassengerId == passengerId);
                
                if (selectedBaggage == null)
                {
                    return NotFound(new ApiResponse(404, $"Baggage with Id {model.BaggageId} does not exist."));
                }

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

            await _baggageRepository.UpdateAsync(changesToSave.ToArray());
            
            var updatedBaggageList = _mapper.Map<List<BaggageOverviewDto>>(changesToSave);

            return Ok(updatedBaggageList);
        }

        /// <summary>
        /// Deletes the selected baggage for a specific passenger.
        /// </summary>
        /// <param name="passengerId">The ID of the passenger.</param>
        /// <param name="baggageIds">The IDs of the baggage to be deleted.</param>
        /// <returns>A <see cref="NoContentResult"/> object if the operation is successful.</returns>
        [HttpDelete("passenger/{passengerId:guid}/delete-baggage")]
        public async Task<ActionResult> DeleteSelectedBaggage(Guid passengerId, [FromBody] List<Guid> baggageIds)
        {
            var selectedBaggage = new List<Baggage>();

            foreach (var baggageId in baggageIds)
            {
                var baggage =
                    await _baggageRepository.GetBaggageByCriteriaAsync(b =>
                        b.Id == baggageId && b.PassengerId == passengerId);
                
                if (baggage == null)
                {
                    return NotFound(new ApiResponse(404, $"Baggage with Id {baggageId} does not exist."));
                }

                selectedBaggage.Add(baggage);
            }

            await _baggageRepository.DeleteAsync(selectedBaggage.ToArray());

            return NoContent();
        }
    }
}

