using System.Linq.Expressions;
using Core.Interfaces;
using Core.PassengerContext.APIS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web.Api.PassengerManagement.Models;
using Web.Errors;

namespace Web.Api.PassengerManagement.Controllers
{
    [ApiController]
    [Route("passenger-management/travel-document")]
    public class TravelDocumentController : ControllerBase
    {
        private readonly IAPISDataRepository _apisDataRepository;
        private readonly ICountryRepository _countryRepository;

        public TravelDocumentController(
            IAPISDataRepository apisDataRepository,
            ICountryRepository countryRepository)
        {
            _apisDataRepository = apisDataRepository;
            _countryRepository = countryRepository;
        }

        private async Task _ProcessTravelDocumentsAsync<TModel>(Guid id, List<JObject> dataList,
            Func<APISData[], Task> saveMethod, Func<Guid, Task<APISData>> getByIdMethod = null)
            where TModel : APISDataModel
        {
            var processedApisDataList = new List<APISData>();
            foreach (var data in dataList)
            {
                var model = JsonConvert.DeserializeObject<TModel>(data.ToString());
                var nationality = await _countryRepository.GetCountryByCriteriaAsync(d =>
                    d.Country3LetterCode == model.Nationality);
                var countryOfIssue = await _countryRepository.GetCountryByCriteriaAsync(d =>
                    d.Country3LetterCode == model.CountryOfIssue);
                if (nationality == null || countryOfIssue == null)
                {
                    var message = (nationality == null && countryOfIssue == null)
                        ?
                        $"Nationality {model.Nationality} and Country of Issue {model.CountryOfIssue}"
                        : (nationality == null)
                            ? $"Nationality {model.Nationality}"
                            : $"Country of Issue {model.CountryOfIssue}";

                    throw new Exception(message + " not found.");
                }

                APISData travelDocument;
                if (getByIdMethod == null) // Add method
                {
                    travelDocument = new APISData(id, nationality.Country2LetterCode, countryOfIssue.Country2LetterCode,
                        model.DocumentNumber, model.DocumentType, model.Gender, model.FirstName, model.LastName,
                        model.DateOfBirth, model.DateOfIssue, model.ExpirationDate);
                }
                else // Edit method
                {
                    travelDocument = (model is EditAPISDataModel dataModel)
                        ? await getByIdMethod(dataModel.APISDataId)
                        : null;

                    if (travelDocument != null)
                    {
                        travelDocument.FirstName = model.FirstName;
                        travelDocument.LastName = model.LastName;
                        travelDocument.Gender = model.Gender;
                        travelDocument.DateOfBirth = model.DateOfBirth;
                        travelDocument.DocumentNumber = model.DocumentNumber;
                        travelDocument.DocumentType = model.DocumentType;
                        travelDocument.CountryOfIssueId = countryOfIssue.Country2LetterCode;
                        travelDocument.DateOfIssue = model.DateOfIssue;
                        travelDocument.ExpirationDate = model.ExpirationDate;
                        travelDocument.NationalityId = nationality.Country2LetterCode;
                    }
                }

                processedApisDataList.Add(travelDocument);
            }

            await saveMethod(processedApisDataList.ToArray());
        }

        /// <summary>
        /// Adds a travel document to a passenger's APIS data.
        /// </summary>
        /// <param name="id">The ID of the passenger</param>
        /// <param name="dataList">The list of travel documents to add</param>
        /// <returns>An HTTP action result with the added APIS data</returns>
        [HttpPost("passenger/{id:guid}/add-document")]
        public async Task<ActionResult<APISData>> AddTravelDocument(Guid id, [FromBody] List<JObject> dataList)
        {
            Func<APISData[], Task> saveMethod = _apisDataRepository.AddAsync;

            try
            {
                await _ProcessTravelDocumentsAsync<AddAPISDataModel>(id, dataList, saveMethod);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        /// <summary>
        /// Edits the travel document for a passenger.
        /// </summary>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="dataList">The list of data for the travel document.</param>
        /// <returns>The updated <see cref="APISData"/> object.</returns>
        [HttpPut("passenger/{id:guid}/edit-document")]
        public async Task<ActionResult<APISData>> EditTravelDocument(Guid id, [FromBody] List<JObject> dataList)
        {
            Func<APISData[], Task> saveMethod = _apisDataRepository.UpdateAsync;

            try
            {
                await _ProcessTravelDocumentsAsync<EditAPISDataModel>(id, dataList, saveMethod, GetByIdMethod);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }

            async Task<APISData> GetByIdMethod(Guid apisDataId) =>
                await _apisDataRepository.GetAPISDataByCriteriaAsync(d => d.Id == apisDataId);
        }

        /// <summary>
        /// Deletes the specified travel documents for a passenger.
        /// </summary>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="apisDataIds">The IDs of the travel documents to delete.</param>
        /// <returns>An ActionResult of APISData.</returns>
        [HttpDelete("passenger/{id:guid}/delete-document")]
        public async Task<ActionResult<APISData>> DeleteTravelDocument(Guid id, [FromBody] List<Guid> apisDataIds)
        {
            var travelDocumentsToDelete = new List<APISData>();

            foreach (var apisDataId in apisDataIds)
            {
                Expression<Func<APISData, bool>> criteria = c => c.PassengerId == id && c.Id == apisDataId;

                var travelDocument = await _apisDataRepository.GetAPISDataByCriteriaAsync(criteria);

                travelDocumentsToDelete.Add(travelDocument);
            }

            if (travelDocumentsToDelete.Count != apisDataIds.Count)
            {
                return BadRequest(new ApiResponse(400, "Invalid travel document IDs."));
            }

            await _apisDataRepository.DeleteAsync(travelDocumentsToDelete.ToArray());

            return NoContent();
        }
    }
}