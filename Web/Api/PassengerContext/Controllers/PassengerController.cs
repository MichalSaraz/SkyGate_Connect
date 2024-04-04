using System.Linq.Expressions;
using AutoMapper;
using Core.BaggageContext;
using Core.BaggageContext.Enums;
using Core.Dtos;
using Core.FlightContext;
using Core.FlightContext.JoinClasses;
using Core.Interfaces;
using Core.PassengerContext;
using Core.PassengerContext.APIS;
using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Core.PassengerContext.JoinClasses;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Web.Api.PassengerContext.Models;
using Web.Errors;

namespace Web.Api.PassengerContext.Controllers
{
    [ApiController]
    [Route("passenger")]
    public class PassengerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITimeProvider _timeProvider;
        private readonly IFlightRepository _flightRepository;
        private readonly IOtherFlightRepository _otherFlightRepository;
        private readonly IBaseFlightRepository _baseFlightRepository;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IBaggageRepository _baggageRepository;
        private readonly IDestinationRepository _destinationRepository;
        private readonly ISSRCodeRepository _sSRCodeRepository;
        private readonly ISpecialServiceRequestRepository _specialServiceRequestRepository;
        private readonly IAPISDataRepository _apisDataRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IPredefinedCommentRepository _predefinedCommentRepository;

        public PassengerController(
            IMapper mapper,
            ITimeProvider timeProvider,
            IFlightRepository flightRepository,
            IOtherFlightRepository otherFlightRepository,
            IBaseFlightRepository baseFlightRepository,
            IPassengerRepository passengerRepository,
            IBaggageRepository baggageRepository,
            IDestinationRepository destinationRepository,
            ISSRCodeRepository sSRCodeRepository,
            ISpecialServiceRequestRepository specialServiceRequestRepository,
            IAPISDataRepository apisDataRepository,
            ICountryRepository countryRepository,
            ICommentRepository commentRepository,
            IPredefinedCommentRepository predefinedCommentRepository)
        {
            _mapper = mapper;
            _timeProvider = timeProvider;
            _flightRepository = flightRepository;
            _otherFlightRepository = otherFlightRepository;
            _baseFlightRepository = baseFlightRepository;
            _passengerRepository = passengerRepository;
            _baggageRepository = baggageRepository;
            _destinationRepository = destinationRepository;
            _sSRCodeRepository = sSRCodeRepository;
            _specialServiceRequestRepository = specialServiceRequestRepository;
            _apisDataRepository = apisDataRepository;
            _countryRepository = countryRepository;
            _commentRepository = commentRepository;
            _predefinedCommentRepository = predefinedCommentRepository;
        }

        /// <summary>
        /// Searches for passengers based on the given search criteria.
        /// </summary>
        /// <param name="data">The search criteria as a JObject.</param>
        /// <returns>Returns a list of Passenger objects that match the search criteria.</returns>
        [HttpPost("search")]
        public async Task<ActionResult<List<Passenger>>> SearchPassengers([FromBody] JObject data)
        {
            var model = new PassengerSearchModel
            {
                //ToDo : ToUpper(), ToLower() apply
                FlightNumber = data["flightNumber"]?.ToString(),
                AirlineId = data["airlineId"]?.ToString(),
                DepartureDate = _timeProvider.ParseDate(data["departureDate"]?.ToString()),
                DocumentNumber = data["documentNumber"]?.ToString(),
                LastName = data["lastName"]?.ToString(),
                PNR = data["pnr"]?.ToString(),
                DestinationFrom = data["destinationFrom"]?.ToString(),
                DestinationTo = data["destinationTo"]?.ToString(),
                SeatNumber = data["seatNumber"]?.ToString()
            };

            if (string.IsNullOrEmpty(model.FlightNumber) || string.IsNullOrEmpty(model.AirlineId) ||
                !model.DepartureDate.HasValue || (string.IsNullOrEmpty(model.DocumentNumber) &&
                                                  string.IsNullOrEmpty(model.LastName) &&
                                                  string.IsNullOrEmpty(model.PNR) &&
                                                  string.IsNullOrEmpty(model.DestinationFrom) &&
                                                  string.IsNullOrEmpty(model.DestinationTo) &&
                                                  string.IsNullOrEmpty(model.SeatNumber)))
            {
                return BadRequest(new ApiResponse(400,
                    "All mandatory plus one optional field must be filled in for the search criteria."));
            }

            Expression<Func<Passenger, bool>> criteria = c =>
                c.Flights.Any(pf =>
                    pf.Flight is Flight && (pf.Flight as Flight).ScheduledFlightId.Substring(2) == model.FlightNumber &&
                    pf.Flight.AirlineId == model.AirlineId &&
                    pf.Flight.DepartureDateTime.Date == model.DepartureDate.Value.Date) &&
                (string.IsNullOrEmpty(model.DocumentNumber) ||
                 c.TravelDocuments.Any(a => a.DocumentNumber == model.DocumentNumber)) &&
                (string.IsNullOrEmpty(model.DestinationFrom) ||
                 c.Flights.Any(bf => bf.Flight.DestinationFromId == model.DestinationFrom)) &&
                (string.IsNullOrEmpty(model.DestinationTo) ||
                 c.Flights.Any(bf => bf.Flight.DestinationToId == model.DestinationTo)) &&
                (string.IsNullOrEmpty(model.SeatNumber) ||
                 c.AssignedSeats.Any(s => s.SeatNumber == model.SeatNumber)) &&
                (string.IsNullOrEmpty(model.LastName) ||
                 c.LastName.Substring(0, model.LastName.Length) == model.LastName) &&
                (string.IsNullOrEmpty(model.PNR) || c.BookingDetails.PNRId == model.PNR);

            var passengers = await _passengerRepository.GetPassengersByCriteriaAsync(criteria);

            var selectedFlight = await _flightRepository.GetFlightByCriteriaAsync(f =>
                f.ScheduledFlightId.Substring(2) == model.FlightNumber && f.AirlineId == model.AirlineId &&
                f.DepartureDateTime.Date == model.DepartureDate.Value.Date);

            var passengersDto = _mapper.Map<List<PassengerOverviewDto>>(passengers, opt =>
            {
                opt.Items["DepartureDateTime"] = selectedFlight.DepartureDateTime;
                opt.Items["FlightId"] = selectedFlight.Id;
            });

            return Ok(passengersDto);
        }

        /// <summary>
        /// Retrieves details of a passenger for a specific flight.
        /// </summary>
        /// <param name="id">The unique identifier of the passenger.</param>
        /// <param name="flightId">The unique identifier of the flight.</param>
        /// <returns>An ActionResult object containing the details of the passenger for the specified flight.
        /// If the passenger is not found, returns a NotFound status with an error message. If the passenger details
        /// are retrieved successfully, returns an Ok status with the passenger details.</returns>
        [HttpGet("{id:guid}/flight/{flightId:guid}/details")]
        public async Task<ActionResult<Passenger>> GetPassengerDetails(Guid id, Guid flightId)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id, false, true);
            var selectedFlight = await _flightRepository.GetFlightByIdAsync(flightId, false);

            var passengerDto = _mapper.Map<PassengerDetailsDto>(passenger, opt =>
            {
                opt.Items["DepartureDateTime"] = selectedFlight.DepartureDateTime;
                opt.Items["FlightId"] = flightId;
            });

            if (passengerDto == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger {id} not found"));
            }

            return Ok(passengerDto);
        }

        /// <summary>
        /// Adds baggage for a passenger on a specific flight.
        /// </summary>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="addBaggageModels">The list of baggage models to add.</param>
        /// <returns>An ActionResult with the added baggage.</returns>
        [HttpPost("{id:guid}/flight/{flightId:guid}/add-baggage")]
        public async Task<ActionResult<Baggage>> AddBaggage(Guid id, Guid flightId,
            [FromBody] List<AddBaggageModel> addBaggageModels)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id);
            var selectedFlight = await _flightRepository.GetFlightByIdAsync(flightId);
            var destination = await _destinationRepository.GetDestinationByCriteriaAsync(d =>
                d.IATAAirportCode == addBaggageModels.FirstOrDefault().FinalDestination);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            var baggageList = new List<Baggage>();

            foreach (var baggageModel in addBaggageModels)
            {
                var newBaggage = new Baggage(passenger.Id, destination.IATAAirportCode, baggageModel.Weight)
                {
                    SpecialBag =
                        baggageModel.SpecialBagType.HasValue
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
        /// <param name="id">The unique identifier of the passenger.</param>
        /// <param name="editBaggageModels">The list of baggage models containing the changes to apply.</param>
        /// <returns>The updated baggage information.</returns>
        [HttpPut("{id:guid}/edit-baggage")]
        public async Task<ActionResult<Baggage>> EditBaggage(Guid id,
            [FromBody] List<EditBaggageModel> editBaggageModels)
        {
            var changesToSave = new List<Baggage>();

            foreach (var model in editBaggageModels)
            {
                var selectedBaggage =
                    await _baggageRepository.GetBaggageByCriteriaAsync(b =>
                        b.Id == model.BaggageId && b.PassengerId == id);

                if (selectedBaggage != null)
                {
                    selectedBaggage.Weight = model.Weight;

                    if (model.SpecialBagType.HasValue)
                    {
                        if (selectedBaggage.SpecialBag == null)
                        {
                            selectedBaggage.SpecialBag = new SpecialBag(model.SpecialBagType.Value,
                                model.Description);
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
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="baggageIds">The list of baggage IDs to delete.</param>
        /// <returns>Returns an ActionResult representing the HTTP response.</returns>
        [HttpDelete("{id:guid}/delete-baggage")]
        public async Task<ActionResult> DeleteSelectedBaggage(Guid id, [FromBody] List<Guid> baggageIds)
        {
            var selectedBaggage = new List<Baggage>();

            foreach (var baggageId in baggageIds)
            {
                var baggage =
                    await _baggageRepository.GetBaggageByCriteriaAsync(b => b.Id == baggageId && b.PassengerId == id);

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

        /// <summary>
        /// Retrieves all the bags of a passenger based on the provided id.
        /// </summary>
        /// <param name="id">The id of the passenger.</param>
        /// <returns>A list of BaggageDetailsDto objects representing the bags of the passenger.</returns>
        [HttpGet("{id:guid}/all-bags")]
        public async Task<ActionResult<List<Baggage>>> GetAllPassengersBags(Guid id)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id, false, true);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            var passengerDto = _mapper.Map<List<BaggageDetailsDto>>(passenger.PassengerCheckedBags);

            return Ok(passengerDto);
        }

        /// <summary>
        /// Adds a connecting flight to a passenger.
        /// </summary>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="flightId">The ID of the flight.</param>
        /// <param name="isInbound">Specifies if the connecting flight is inbound.</param>
        /// <param name="addConnectingFlightModels">The list of connecting flights to add.</param>
        /// <returns>An ActionResult of type BaseFlight.</returns>
        [HttpPost("{id:guid}/flight/{flightId:guid}/add-connecting-flight")]
        public async Task<ActionResult<BaseFlight>> AddConnectingFlight(Guid id, Guid flightId, bool isInbound,
            [FromBody] List<AddConnectingFlightModel> addConnectingFlightModels)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            var currentPassengerFlights = passenger.Flights.Select(pf => pf.Flight).ToList();
            var currentFlight = await _flightRepository.GetFlightByIdAsync(flightId, false);

            foreach (var connectingFlightModel in addConnectingFlightModels)
            {
                var parsedDepartureDateTime = _timeProvider.ParseDate(connectingFlightModel.DepartureDate).Value;

                var lastFlight = currentPassengerFlights[^1];

                var connectingFlight = await _GetOrCreateFlightAsync(connectingFlightModel, parsedDepartureDateTime);

                switch (isInbound)
                {
                    case false when
                        (connectingFlight as Flight)?.DepartureDateTime < (lastFlight as Flight)?.DepartureDateTime ||
                        connectingFlight.DepartureDateTime.Date < lastFlight.DepartureDateTime.Date ||
                        connectingFlight.DepartureDateTime > lastFlight.DepartureDateTime.AddDays(2):
                    {
                        return BadRequest(new ApiResponse(400,
                            "Connecting flight must be within 24 hours from last arrival."));
                    }
                    case true when ((connectingFlight as Flight)?.ArrivalDateTime > currentFlight.DepartureDateTime ||
                                    connectingFlight.DepartureDateTime.Date > currentFlight.DepartureDateTime.Date ||
                                    connectingFlight.DepartureDateTime < currentFlight.DepartureDateTime.AddDays(-2)):
                    {
                        return BadRequest(new ApiResponse(400,
                            "Inbound flight cannot be earlier than 24 hours before next departure"));
                    }
                }

                if (currentPassengerFlights.Contains(connectingFlight))
                {
                    return BadRequest(new ApiResponse(400, "Flight is already in passenger's itinerary."));
                }

                currentPassengerFlights.Add(connectingFlight);

                var newPassengerFlight = new PassengerFlight(id, currentPassengerFlights.Last().Id,
                    connectingFlightModel.FlightClass);

                passenger.Flights.Add(newPassengerFlight);
            }

            await _passengerRepository.UpdateAsync(passenger);

            return Ok();
        }

        private async Task<BaseFlight> _GetOrCreateFlightAsync(AddConnectingFlightModel connectingFlightModel,
            DateTime parsedDepartureDateTime)
        {
            var flightCriteria = _BuildFlightCriteria(connectingFlightModel, parsedDepartureDateTime);
            var otherFlightCriteria = _BuildOtherFlightCriteria(connectingFlightModel, parsedDepartureDateTime);

            var connectingFlight = await _flightRepository.GetFlightByCriteriaAsync(flightCriteria, true);
            if (connectingFlight != null) return connectingFlight;

            var otherFlight = await _otherFlightRepository.GetOtherFlightByCriteriaAsync(otherFlightCriteria, true);
            if (otherFlight != null) return otherFlight;

            otherFlight = new OtherFlight(connectingFlightModel.FlightNumber, parsedDepartureDateTime, null,
                connectingFlightModel.DestinationFrom, connectingFlightModel.DestinationTo,
                connectingFlightModel.AirlineId);

            await _flightRepository.AddAsync(otherFlight);
            return otherFlight;
        }

        private static Expression<Func<Flight, bool>> _BuildFlightCriteria(AddConnectingFlightModel connectingFlightModel,
            DateTime parsedDepartureDateTime)
        {
            return f => f.AirlineId == connectingFlightModel.AirlineId &&
                        f.ScheduledFlightId.Substring(2) == connectingFlightModel.FlightNumber &&
                        f.DepartureDateTime.Date == parsedDepartureDateTime.Date &&
                        f.DestinationFromId == connectingFlightModel.DestinationFrom &&
                        f.DestinationToId == connectingFlightModel.DestinationTo;
        }

        private static Expression<Func<OtherFlight, bool>> _BuildOtherFlightCriteria(
            AddConnectingFlightModel connectingFlightModel, DateTime parsedDepartureDateTime)
        {
            return of => of.AirlineId == connectingFlightModel.AirlineId &&
                         of.FlightNumber == connectingFlightModel.FlightNumber &&
                         of.DepartureDateTime.Date == parsedDepartureDateTime.Date &&
                         of.DestinationFromId == connectingFlightModel.DestinationFrom &&
                         of.DestinationToId == connectingFlightModel.DestinationTo;
        }

        /// <summary>
        /// Deletes the connecting flight for a passenger.
        /// </summary>
        /// <param name="id">The id of the passenger.</param>
        /// <param name="flightId">The id of the current flight.</param>
        /// <param name="flightIds">The list of flight ids to delete.</param>
        /// <returns>Returns an ActionResult of type BaseFlight.</returns>
        [HttpDelete("{id:guid}/flight/{flightId:guid}/delete-connecting-flight")]
        public async Task<ActionResult<BaseFlight>> DeleteConnectingFlight(Guid id, Guid flightId,
            [FromBody] List<Guid> flightIds)
        {
            var passenger = await _passengerRepository.GetPassengerByIdAsync(id);

            if (passenger == null)
            {
                return NotFound(new ApiResponse(404, $"Passenger with Id {id} was not found."));
            }

            var flightsToDelete = await _baseFlightRepository.GetFlightsByCriteriaAsync(f =>
                flightIds.Contains(f.Id) && f.Id != flightId, true);

            if (flightsToDelete.Count == 0)
            {
                return BadRequest(new ApiResponse(400, "Invalid flight IDs."));
            }

            // Odstranění letů z kolekce a jejich smazání
            passenger.Flights.RemoveAll(pf => flightsToDelete.Contains(pf.Flight));
            await _passengerRepository.UpdateAsync(passenger);

            foreach (var flight in flightsToDelete)
            {
                if (flight.ListOfBookedPassengers.Count == 0)
                {
                    await _baseFlightRepository.DeleteAsync(flight);
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Adds special service requests for a passenger on a specific flight.
        /// </summary>
        /// <param name="id">The ID of the passenger.</param>
        /// <param name="requestData">The special service request data.</param>
        /// <returns>An ActionResult representing the HTTP status code and result of the operation.</returns>
        [HttpPost("{id:guid}/add-special-service-request")]
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
        /// <returns>The result of the deletion operation.</returns>
        [HttpDelete("{id:guid}/delete-special-service-request")]
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
                        ? $"Nationality {model.Nationality} and Country of Issue {model.CountryOfIssue}"
                        : (nationality == null)
                            ? $"Nationality {model.Nationality}"
                            : $"Country of Issue {model.CountryOfIssue}";
                    
                    throw new Exception(message + " not found.");
                }

                APISData travelDocument;
                if (getByIdMethod == null) // Add method
                {
                    travelDocument = new APISData(id,
                        nationality.Country2LetterCode,
                        countryOfIssue.Country2LetterCode,
                        model.DocumentNumber,
                        model.DocumentType,
                        model.Gender,
                        model.FirstName,
                        model.LastName,
                        model.DateOfBirth,
                        model.DateOfIssue,
                        model.ExpirationDate);
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
        [HttpPost("{id:guid}/add-travel-document")]
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
        [HttpPut("{id:guid}/edit-travel-document")]
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
        [HttpDelete("{id:guid}/delete-travel-document")]
        public async Task<ActionResult<APISData>> DeleteTravelDocument(Guid id,
            [FromBody] List<Guid> apisDataIds)
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

        [HttpPost("{id:guid}/add-comment")]
        public async Task<ActionResult<Comment>> AddComment(Guid id, CommentTypeEnum commentType,
            [FromBody] JObject data, string predefineCommentId = null)
        {
            var flightIds = data["flightIds"]?.ToObject<List<Guid>>();
            var text = data["text"]?.ToString();

            Comment comment;

            if (!string.IsNullOrEmpty(predefineCommentId))
            {
                var predefinedComment = await _predefinedCommentRepository.GetPredefinedCommentByIdAsync(predefineCommentId);

                if (predefinedComment == null)
                    return BadRequest(new ApiResponse(400, "Predefined comment not found."));

                var existingComment = await _commentRepository.GetCommentByCriteriaAsync(c => 
                    c.PassengerId == id && c.PredefinedCommentId == predefineCommentId);

                if (existingComment != null)
                    return BadRequest(new ApiResponse(400, "Predefined comment already exists."));

                comment = new Comment(id, predefineCommentId, predefinedComment.Text);
            }
            else
                comment = new Comment(id, commentType, text);

            await _commentRepository.AddAsync(comment);

            if (flightIds == null)
                return BadRequest(new ApiResponse(400, "Flight IDs must be provided."));

            foreach (var flightId in flightIds)
            {
                var newFlightComment = new FlightComment(comment.Id, flightId);
                comment.LinkedToFlights.Add(newFlightComment);
            }

            await _commentRepository.UpdateAsync(comment);

            return Ok();
        }

        [HttpDelete("{id:guid}/delete-comment")]
        public async Task<ActionResult<Comment>> DeleteComment(Guid id, [FromBody] Dictionary<string, List<Guid>> commentIds)
        {
            var commentsToDelete = new HashSet<Comment>();

            foreach (var flight in commentIds.Keys)
            {
                var commentIdsList = commentIds[flight];

                foreach (var commentId in commentIdsList)
                {
                    var comment = await _commentRepository.GetCommentByIdAsync(commentId);

                    if (comment == null)
                        return BadRequest(new ApiResponse(400, $"Comment with ID {commentId} does not exist."));

                    if (comment.LinkedToFlights.All(f => f.FlightId != Guid.Parse(flight)) && comment.LinkedToFlights.Count > 0)
                        return BadRequest(new ApiResponse(400, $"Comment with ID {commentId} is not linked to flight with ID {flight}"));

                    comment.LinkedToFlights.RemoveAll(f => f.FlightId == Guid.Parse(flight));

                    if (!commentsToDelete.Any(c => c.Id == commentId) && comment.LinkedToFlights.Count == 0)
                        commentsToDelete.Add(comment);
                }
            }

            foreach (var comment in commentsToDelete)
            {
                if (comment.LinkedToFlights.Count == 0)
                    await _commentRepository.DeleteAsync(comment);
                else
                    await _commentRepository.UpdateAsync(comment);
            }

            return commentsToDelete.Any()
                ? NoContent()
                : Ok();
        }

        [HttpDelete("{id:guid}/mark-comment-as-read")]
        public async Task<ActionResult<Comment>> MarkGateCommentAsRead(Guid id, [FromBody] Guid commentId)
        {
            var comment = await _commentRepository.GetCommentByCriteriaAsync(c =>
                c.Id == commentId && c.CommentType == CommentTypeEnum.Gate);

            if (comment == null)
                return BadRequest(new ApiResponse(400, $"Gate comment with ID {commentId} does not exist."));

            await _commentRepository.DeleteAsync(comment);

            return NoContent();
        }
    }
}