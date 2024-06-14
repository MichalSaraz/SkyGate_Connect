using System.Linq.Expressions;
using AutoMapper;
using Core.Dtos;
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
        private readonly IMapper _mapper;

        public TravelDocumentController(
            IAPISDataRepository apisDataRepository,
            ICountryRepository countryRepository,
            IMapper mapper)
        {
            _apisDataRepository = apisDataRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        private async Task<ActionResult<List<APISDataDto>>> _ProcessTravelDocumentsAsync<TModel>(Guid id, List<JObject> dataList,
            Func<APISData[], Task> saveMethod, Func<Guid, Task<APISData>>? getByIdMethod = null)
            where TModel : APISDataModel
        {
            var processedApisDataList = new List<APISData>();
            foreach (var data in dataList)
            {
                var model = JsonConvert.DeserializeObject<TModel>(data.ToString());
                
                if (model == null)
                {
                    throw new Exception("Invalid data.");
                }
                
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

                APISData? travelDocument;
                if (getByIdMethod == null) // Add method
                {
                    travelDocument = new APISData(id, nationality.Country2LetterCode,
                        countryOfIssue.Country2LetterCode,
                        model.DocumentNumber, model.DocumentType, model.Gender, model.FirstName, model.LastName,
                        model.DateOfBirth, model.DateOfIssue, model.ExpirationDate);
                }
                else // Edit method
                {
                    travelDocument = model is EditAPISDataModel dataModel
                        ? await getByIdMethod(dataModel.APISDataId)
                        : null;
                    
                    if (travelDocument == null)
                    {
                        throw new Exception("APIS data not found.");
                    }

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

                processedApisDataList.Add(travelDocument);
            }

            await saveMethod(processedApisDataList.ToArray());
            
            var apisDataDto = _mapper.Map<List<APISDataDto>>(processedApisDataList);

            return Ok(apisDataDto);
        }

        /// <summary>
        /// Adds travel document(s) to a passenger's APIS data.
        /// </summary>
        /// <param name="id">The ID of the passenger</param>
        /// <param name="dataList">The list of travel documents to add</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /passenger/3F2504E0-4F89-41D3-9A0C-0305E82C3301/add-document
        ///     {
        ///         "firstName": "John",
        ///         "lastName": "Doe",
        ///         "gender": "M",
        ///         "dateOfBirth": "12JUL2011",
        ///         "documentNumber": "16543415",
        ///         "documentType": "NormalPassport",
        ///         "countryOfIssue": "NOR",
        ///         "dateOfIssue": "14AUG2020",
        ///         "expirationDate": "14AUG2030",
        ///         "nationality": "NOR"
        ///     }
        ///
        /// </remarks>
        /// <returns>A list of added travel documents with APIS data</returns>
        [HttpPost("passenger/{id:guid}/add-document")]
        public async Task<ActionResult<List<APISDataDto>>> AddTravelDocument(Guid id, [FromBody] List<JObject> dataList)
        {
            Func<APISData[], Task> saveMethod = _apisDataRepository.AddAsync;

            try
            {
                var addedApisData = await _ProcessTravelDocumentsAsync<AddAPISDataModel>(id, dataList, saveMethod);
                return Ok(addedApisData);
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        /// <summary>
        /// Edits travel document(s) for a passenger.
        /// </summary>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="dataList">The list of data for the travel document.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /passenger/3F2504E0-4F89-41D3-9A0C-0305E82C3301/edit-document
        ///     {
        ///         "apisDataId": "c9a46b82-b1f8-4d0e-ac61-28e7debc2c03",
        ///         "firstName": "John",
        ///         "lastName": "Doe",
        ///         "gender": "M",
        ///         "dateOfBirth": "12JUL2011",
        ///         "documentNumber": "16543415",
        ///         "documentType": "NormalPassport",
        ///         "countryOfIssue": "NOR",
        ///         "dateOfIssue": "14AUG2020",
        ///         "expirationDate": "14AUG2030",
        ///         "nationality": "NOR"
        ///     }
        ///
        /// </remarks>
        /// <returns>A list of edited travel documents with APIS data.</returns>
        [HttpPut("passenger/{id:guid}/edit-document")]
        public async Task<ActionResult<List<APISDataDto>>> EditTravelDocument(Guid id, [FromBody] List<JObject> dataList)
        {
            Func<APISData[], Task> saveMethod = _apisDataRepository.UpdateAsync;

            try
            {
                var editedApisData = await _ProcessTravelDocumentsAsync<EditAPISDataModel>(id, dataList, saveMethod, GetByIdMethod);
                return Ok(editedApisData);
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
        /// <returns>An ActionResult</returns>
        [HttpDelete("passenger/{id:guid}/delete-document")]
        public async Task<ActionResult> DeleteTravelDocument(Guid id, [FromBody] List<Guid> apisDataIds)
        {
            var travelDocumentsToDelete = new List<APISData>();

            foreach (var apisDataId in apisDataIds)
            {
                Expression<Func<APISData, bool>> criteria = c => c.PassengerId == id && c.Id == apisDataId;

                var travelDocument = await _apisDataRepository.GetAPISDataByCriteriaAsync(criteria);
                
                if (travelDocument == null)
                {
                    return NotFound(new ApiResponse(404, $"Travel document {apisDataId} not found."));
                }

                travelDocumentsToDelete.Add(travelDocument);
            }

            await _apisDataRepository.DeleteAsync(travelDocumentsToDelete.ToArray());

            return NoContent();
        }
    }
}