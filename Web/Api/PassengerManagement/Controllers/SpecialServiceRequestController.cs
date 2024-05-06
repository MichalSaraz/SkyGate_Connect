using Core.Interfaces;
using Core.PassengerContext;
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

        public SpecialServiceRequestController(
            IPassengerRepository passengerRepository,
            ISSRCodeRepository sSRCodeRepository,
            ISpecialServiceRequestRepository specialServiceRequestRepository)
        {
            _passengerRepository = passengerRepository;
            _sSRCodeRepository = sSRCodeRepository;
            _specialServiceRequestRepository = specialServiceRequestRepository;
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
        /// <returns>An ActionResult representing the HTTP status code and result of the operation.</returns>
        [HttpPost("passenger/{id:guid}/add-request")]
        public async Task<ActionResult<Passenger>> AddSpecialServiceRequest(Guid id,
            [FromBody] List<JObject> requestData)
        {
            foreach (var request in requestData)
            {
                var flightIds = request["flightIds"]?.ToObject<List<Guid>>();
                var specialServiceRequests = new List<SpecialServiceRequest>();

                var ssrData = request["specialServiceRequests"];
                
                if (ssrData != null)
                {
                    foreach (var ssrRequest in ssrData)
                    {
                        var SSRCode = await _sSRCodeRepository.GetSSRCodeAsync(ssrRequest["SSRCode"]?.ToString());
                        var freeText = ssrRequest["freeText"]?.ToString();

                        if (SSRCode == null)
                        {
                            return BadRequest(new ApiResponse(400,
                                "SSR must be filled in for the special service request."));
                        }

                        if (SSRCode.IsFreeTextMandatory && string.IsNullOrEmpty(freeText))
                        {
                            return BadRequest(new ApiResponse(400, "FreeText is required for this SSRCode."));
                        }

                        //ToDo: Add validation for adding INFT SSR
                        if (flightIds != null)
                        {
                            foreach (var iteratedFlightId in flightIds)
                            {
                                var specialServiceRequest =
                                    new SpecialServiceRequest(SSRCode.Code, iteratedFlightId, id, freeText);
                                specialServiceRequests.Add(specialServiceRequest);
                            }
                        }
                    }
                }

                await _specialServiceRequestRepository.AddAsync(specialServiceRequests.ToArray());
            }

            return Ok();
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
        /// <returns>The result of the deletion operation.</returns>
        [HttpDelete("passenger/{id:guid}/delete-request")]
        public async Task<ActionResult<Passenger>> DeleteSpecialServiceRequest(Guid id,
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

            await _specialServiceRequestRepository.DeleteAsync(ssrToDeleteBatch.ToArray());

            return NoContent();
        }
    }
}