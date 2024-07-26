using System.Linq.Expressions;
using AutoMapper;
using Core.Dtos;
using Core.HistoryTracking;
using Core.HistoryTracking.Enums;
using Core.Interfaces;
using Core.PassengerContext.APIS;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IActionHistoryRepository _actionHistoryRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly IMapper _mapper;

        public TravelDocumentController(
            IAPISDataRepository apisDataRepository,
            ICountryRepository countryRepository,
            IActionHistoryRepository actionHistoryRepository,
            ITimeProvider timeProvider,
            IMapper mapper)
        {
            _apisDataRepository = apisDataRepository;
            _countryRepository = countryRepository;
            _actionHistoryRepository = actionHistoryRepository;
            _timeProvider = timeProvider;
            _mapper = mapper;
        }

        /// <summary>
        /// Processes travel documents asynchronously.
        /// </summary>
        /// <typeparam name="TModel">The type of the travel document model.</typeparam>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="models">The list of JSON objects representing the travel documents.</param>
        /// <param name="saveMethod">The save method to be called with the processed travel documents.</param>
        /// <param name="getByIdMethod">The method to get a travel document by ID. This is optional and needed only for
        /// edit operations.</param>
        /// <returns>An ActionResult containing a list of APISDataDto objects representing the processed travel
        /// documents.</returns>
        private async Task<ActionResult<List<APISDataDto>>> _ProcessTravelDocumentsAsync<TModel>(Guid id,
            List<TModel> models, Func<APISData[], Task> saveMethod, Func<Guid, Task<APISData>>? getByIdMethod = null)
            where TModel : APISDataModel
        {
            var processedApisDataList = new List<APISData>();
            var oldValues = new List<APISData>();
            
            foreach (var model in models)
            {
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
                    travelDocument = new APISData(id, nationality.Country2LetterCode, countryOfIssue.Country2LetterCode,
                        model.DocumentNumber, model.DocumentType, model.Gender, model.FirstName, model.LastName,
                        _timeProvider.ParseDate(model.DateOfBirth, true),
                        _timeProvider.ParseDate(model.DateOfIssue, true), 
                        _timeProvider.ParseDate(model.ExpirationDate, true));
                }
                else // Edit method
                {
                    travelDocument = model is EditAPISDataModel dataModel
                        ? await getByIdMethod(dataModel.APISDataId)
                        : null;
                    
                    if (travelDocument == null) throw new Exception("APIS data not found.");
                    
                    oldValues.Add(travelDocument);

                    travelDocument.FirstName = model.FirstName;
                    travelDocument.LastName = model.LastName;
                    travelDocument.Gender = model.Gender;
                    travelDocument.DateOfBirth = _timeProvider.ParseDate(model.DateOfBirth, true);
                    travelDocument.DocumentNumber = model.DocumentNumber;
                    travelDocument.DocumentType = model.DocumentType;
                    travelDocument.CountryOfIssueId = countryOfIssue.Country2LetterCode;
                    travelDocument.DateOfIssue = _timeProvider.ParseDate(model.DateOfIssue, true);
                    travelDocument.ExpirationDate = _timeProvider.ParseDate(model.ExpirationDate, true);
                    travelDocument.NationalityId = nationality.Country2LetterCode;
                }

                processedApisDataList.Add(travelDocument);
            }
            
            ActionHistory<object> record;
            var apisDataDto = _mapper.Map<List<APISDataDto>>(processedApisDataList);
            
            if (getByIdMethod != null)
            {
                record = new ActionHistory<object>(ActionTypeEnum.Updated, id, nameof(APISData), apisDataDto,
                    _mapper.Map<List<APISDataDto>>(oldValues));
                
                await _actionHistoryRepository.AddAsync(record);
            }
            else
            {
                record = new ActionHistory<object>(ActionTypeEnum.Created, id, nameof(APISData), apisDataDto);
                
                await _actionHistoryRepository.AddAsync(record);
            }
            
            await saveMethod(processedApisDataList.ToArray());

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
        ///     [
        ///         {
        ///             "firstName": "John",
        ///             "lastName": "Doe",
        ///             "gender": "M",
        ///             "dateOfBirth": "12JUL2011",
        ///             "documentNumber": "16543415",
        ///             "documentType": "NormalPassport",
        ///             "countryOfIssue": "NOR",
        ///             "dateOfIssue": "14AUG2020",
        ///             "expirationDate": "14AUG2030",
        ///             "nationality": "NOR"
        ///         }
        ///     ]
        ///
        /// </remarks>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="APISDataDto"/> objects. If the request is invalid, returns <see cref="NotFoundResult"/> with an
        /// error message.</returns>
        [HttpPost("passenger/{id:guid}/add-document")]
        public async Task<ActionResult<List<APISDataDto>>> AddTravelDocument(Guid id,
            [FromBody] List<AddAPISDataModel> dataList)
        {
            Func<APISData[], Task> saveMethod = _apisDataRepository.AddAsync;

            try
            {
                var addedApisData = await _ProcessTravelDocumentsAsync(id, dataList, saveMethod);
                
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
        ///     [
        ///         {
        ///             "apisDataId": "c9a46b82-b1f8-4d0e-ac61-28e7debc2c03",
        ///             "firstName": "John",
        ///             "lastName": "Doe",
        ///             "gender": "M",
        ///             "dateOfBirth": "12JUL2011",
        ///             "documentNumber": "16543415",
        ///             "documentType": "NormalPassport",
        ///             "countryOfIssue": "NOR",
        ///             "dateOfIssue": "14AUG2020",
        ///             "expirationDate": "14AUG2030",
        ///             "nationality": "NOR"
        ///         }
        ///     ]
        ///
        /// </remarks>
        /// <returns>An <see cref="ActionResult{T}"/> containing a list <see cref="List{T}"/> of
        /// <see cref="APISDataDto"/> objects. If the request is invalid, returns <see cref="NotFoundResult"/> with an
        /// error message.</returns>
        [HttpPut("passenger/{id:guid}/edit-document")]
        public async Task<ActionResult<List<APISDataDto>>> EditTravelDocument(Guid id,
            [FromBody] List<EditAPISDataModel> dataList)
        {
            Func<APISData[], Task> saveMethod = _apisDataRepository.UpdateAsync;

            try
            {
                var editedApisData = await _ProcessTravelDocumentsAsync(id, dataList, saveMethod, GetByIdMethod);
                
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
        /// <returns>A <see cref="NoContentResult"/> if the comment was deleted successfully.</returns>
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
            
            var record = new ActionHistory<object?>(ActionTypeEnum.Deleted, id, nameof(APISData), null,
                _mapper.Map<List<APISDataDto>>(travelDocumentsToDelete));
            
            await _actionHistoryRepository.AddAsync(record);
            await _apisDataRepository.DeleteAsync(travelDocumentsToDelete.ToArray());

            return NoContent();
        }
    }
}