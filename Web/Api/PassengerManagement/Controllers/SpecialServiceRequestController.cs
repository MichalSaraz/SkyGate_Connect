using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.HistoryTracking;
using Core.HistoryTracking.Enums;
using Core.Interfaces;
using Core.PassengerContext.JoinClasses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Web.Errors;

namespace Web.Api.PassengerManagement.Controllers
{
    [ApiController]
    [Route("passenger-management/special-service-request")]
    public class SpecialServiceRequestController : ControllerBase
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly ISSRCodeRepository _sSRCodeRepository;
        private readonly ISpecialServiceRequestRepository _specialServiceRequestRepository;
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;

        public SpecialServiceRequestController(
            IPassengerRepository passengerRepository,
            ISSRCodeRepository sSRCodeRepository,
            ISpecialServiceRequestRepository specialServiceRequestRepository,
            IActionHistoryRepository actionHistoryRepository,
            IFlightRepository flightRepository,
            IMapper mapper)
        {
            _passengerRepository = passengerRepository;
            _sSRCodeRepository = sSRCodeRepository;
            _specialServiceRequestRepository = specialServiceRequestRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _flightRepository = flightRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds special service requests for a passenger on a specific flight.
        /// </summary>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="requestData">The special service request data.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /passenger/3F2504E0-4F89-41D3-9A0C-0305E82C3301/add-request
        ///     [
        ///         {
        ///             "flightIds": ["58590667-cec5-4a89-a58e-b1572d7086e9", "99382844-5d43-4051-97a7-07858f76bc7e"],
        ///             "specialServiceRequests": [
        ///                 {
        ///                     "SSRCode": "WCHR",
        ///                     "freeText": null
        ///                 },
        ///                 {
        ///                     "SSRCode": "WCMP",
        ///                     "freeText": "dimensions 50x50x60"
        ///                 }
        ///             ]
        ///         }
        ///     ]
        ///
        /// </remarks>
        /// <returns>Returns an <see cref="ActionResult{T}"/> containing a <see cref="SpecialServiceRequestDto"/>
        /// object.</returns>
        [HttpPost("passenger/{id:guid}/add-request")]
        public async Task<ActionResult<SpecialServiceRequestDto>> AddSpecialServiceRequest(Guid id,
            [FromBody] List<JObject> requestData)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id, false, true);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            var specialServiceRequests = new List<SpecialServiceRequest>();

            foreach (var request in requestData)
            {
                var flightIds = request["flightIds"]?.ToObject<List<Guid>>();

                var ssrData = request["specialServiceRequests"];

                if (ssrData != null)
                {
                    foreach (var ssrRequest in ssrData)
                    {
                        var SSRCode = await _sSRCodeRepository.GetSSRCodeAsync(ssrRequest["SSRCode"]?.ToString());
                        var freeText = ssrRequest["freeText"]?.ToString();

                        if (SSRCode == null)
                        {
                            return NotFound(new ApiResponse(404, "SSRCode not found."));
                        }

                        if (SSRCode.IsFreeTextMandatory && string.IsNullOrEmpty(freeText))
                        {
                            return BadRequest(new ApiResponse(400, "FreeText is required for this SSRCode."));
                        }

                        if (flightIds != null)
                        {
                            foreach (var flightId in flightIds)
                            {
                                if (!await _flightRepository.ExistsAsync(flightId))
                                {
                                    return NotFound(new ApiResponse(404,
                                        $"Flight with Id {flightId} not found."));
                                }

                                var isSSRAlreadyExists = passenger.SpecialServiceRequests.Any(ssr =>
                                    ssr.FlightId == flightId && ssr.SSRCodeId == SSRCode.Code);
                                if (isSSRAlreadyExists)
                                {
                                    return BadRequest(new ApiResponse(400,
                                        $"SSR {SSRCode.Code} already exists for passenger {id} on flight {flightId}"));
                                }

                                var specialServiceRequest =
                                    new SpecialServiceRequest(SSRCode.Code, flightId, id, freeText);
                                specialServiceRequests.Add(specialServiceRequest);
                            }
                        }
                    }
                }
            }

            var specialServiceRequestsDto = _mapper.Map<List<SpecialServiceRequestDto>>(specialServiceRequests);

            foreach (var request in specialServiceRequestsDto)
            {
                var flight = passenger.Flights.First(f => f.FlightId == request.FlightId).Flight as Flight;
                request.FlightNumber = flight?.ScheduledFlightId;
            }
            
            var record = new ActionHistory<object>(ActionTypeEnum.Created, id, nameof(SpecialServiceRequest),
                specialServiceRequestsDto);
                
            await _actionHistoryRepository.AddAsync(record);
            await _specialServiceRequestRepository.AddAsync(specialServiceRequests.ToArray());

            return Ok(specialServiceRequestsDto);
        }

        /// <summary>
        /// Deletes the special service requests for a passenger.
        /// </summary>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="ssrCodesToDelete">A dictionary where the key is the flight ID and the value is
        /// a list of SSR codes to delete.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /passenger/3F2504E0-4F89-41D3-9A0C-0305E82C3301/delete-request
        ///     {
        ///         "58590667-cec5-4a89-a58e-b1572d7086e9": ["WCHR", "WCMP"],
        ///         "99382844-5d43-4051-97a7-07858f76bc7e": ["WCHR", "WCMP"]
        ///     }
        ///
        /// </remarks>
        /// <returns>A <see cref="NoContentResult"/> if the request was successful.</returns>
        [HttpDelete("passenger/{id:guid}/delete-request")]
        public async Task<ActionResult> DeleteSpecialServiceRequest(Guid id,
            [FromBody] Dictionary<string, List<string>> ssrCodesToDelete)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id, true, true);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            var ssrToDeleteBatch = new List<SpecialServiceRequest>();

            foreach (var flight in ssrCodesToDelete.Keys)
            {
                var ssrCodes = ssrCodesToDelete[flight];
                var flightId = Guid.Parse(flight);

                var ssrToDelete = passenger.SpecialServiceRequests
                    .Where(ssr => ssr.FlightId == flightId && ssrCodes.Contains(ssr.SSRCodeId))
                    .ToList();

                if (ssrToDelete.Count != ssrCodes.Count)
                {
                    return BadRequest(new ApiResponse(400, "Invalid SSR codes."));
                }

                ssrToDeleteBatch.AddRange(ssrToDelete);
            }
            
            var record = new ActionHistory<object?>(ActionTypeEnum.Deleted, id, nameof(SpecialServiceRequest), null,
                _mapper.Map<List<SpecialServiceRequestDto>>(ssrToDeleteBatch));
            
            await _actionHistoryRepository.AddAsync(record);
            await _specialServiceRequestRepository.DeleteAsync(ssrToDeleteBatch.ToArray());

            return NoContent();
        }
    }
}