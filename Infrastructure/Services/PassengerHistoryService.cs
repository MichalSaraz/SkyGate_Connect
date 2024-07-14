using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.HistoryTracking;
using Core.HistoryTracking.Enums;
using Core.Interfaces;
using Core.PassengerContext;
using Core.SeatingContext;

namespace Infrastructure.Services;

public class PassengerHistoryService : IPassengerHistoryService
{
    private readonly IBasePassengerOrItemRepository _basePassengerOrItemRepository;
    private readonly IPassengerDtoMappingService _passengerDtoMappingService;
    private readonly IActionHistoryRepository _actionHistoryRepository;
    private readonly IMapper _mapper;

    public PassengerHistoryService(
        IBasePassengerOrItemRepository basePassengerOrItemRepository,
        IPassengerDtoMappingService passengerDtoMappingService,
        IActionHistoryRepository actionHistoryRepository,
        IMapper mapper)
    {
        _basePassengerOrItemRepository = basePassengerOrItemRepository;
        _passengerDtoMappingService = passengerDtoMappingService;
        _actionHistoryRepository = actionHistoryRepository;
        _mapper = mapper;
    }

    public async Task<List<object>> SavePassengerOrItemActionsToPassengerHistoryAsync(
        IReadOnlyList<BasePassengerOrItem> oldValues, IReadOnlyList<BasePassengerOrItem> passengerOrItems,
        BaseFlight selectedFlight, bool displayDetails = true)
    {
        oldValues = oldValues.OrderBy(x => x.Id).ToList();
        passengerOrItems = passengerOrItems.OrderBy(x => x.Id).ToList();

        var oldValuesDto =
            _passengerDtoMappingService.MapPassengerOrItemsToDto(oldValues, selectedFlight, displayDetails);
        var passengerOrItemsDto =
            _passengerDtoMappingService.MapPassengerOrItemsToDto(passengerOrItems, selectedFlight, displayDetails);

        for (int i = 0; i < passengerOrItems.Count; i++)
        {
            var record = new ActionHistory<object>(ActionTypeEnum.Updated, passengerOrItems[i].Id, nameof(Passenger),
                passengerOrItemsDto[i], oldValuesDto[i]);

            await _actionHistoryRepository.AddAsync(record);
        }

        return passengerOrItemsDto;
    }

    public async Task<List<SeatDto>> SaveSeatActionsToPassengerHistoryAsync(Guid flightId,
        IReadOnlyList<BasePassengerOrItem> oldValues)
    {
        var passengerOrItems = await _basePassengerOrItemRepository.GetBasePassengerOrItemsByCriteriaAsync(p => 
            oldValues.Select(x => x.Id).Contains(p.Id), false);
        
        oldValues = oldValues.OrderBy(x => x.Id).ToList();
        passengerOrItems = passengerOrItems.OrderBy(x => x.Id).ToList();

        var oldValuesDto = oldValues.Select(p =>
                _mapper.Map<SeatDto>(p.AssignedSeats.FirstOrDefault(s => s.FlightId == flightId)))
            .ToList();
        var seatDto = passengerOrItems.Select(p =>
                _mapper.Map<SeatDto>(p.AssignedSeats.FirstOrDefault(s => s.FlightId == flightId)))
            .ToList();
        
        var tasks = new List<Task>();
        
        for (int i = 0; i < passengerOrItems.Count; i++)
        {
            var record = new ActionHistory<object>(ActionTypeEnum.Updated, passengerOrItems[i].Id, nameof(Seat),
                seatDto[i], oldValuesDto[i]);

            tasks.Add(_actionHistoryRepository.AddAsync(record));
        }

        await Task.WhenAll(tasks);

        return seatDto;
    }
}