using AutoMapper;
using Core.Dtos;
using Core.FlightContext;
using Core.Interfaces;
using Core.PassengerContext;

namespace Infrastructure.Services;

public class PassengerDtoMappingService : IPassengerDtoMappingService
{
    private readonly IMapper _mapper;

    public PassengerDtoMappingService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public object MapSinglePassengerOrItemToDto(object obj, BaseFlight flight,
        bool displayDetails = true)
    {
        object dto = obj switch
        {
            Passenger passenger => (displayDetails)
                ? _mapper.Map<PassengerDetailsDto>(passenger, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = flight.Id;
                })
                : _mapper.Map<PassengerOrItemOverviewDto>(passenger, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = flight.Id;
                }),
            Infant infant => (displayDetails)
                ? _mapper.Map<InfantDetailsDto>(infant, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = flight.Id;
                })
                : _mapper.Map<InfantOverviewDto>(infant, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = flight.Id;
                }),
            CabinBaggageRequiringSeat or ExtraSeat when displayDetails => 
                _mapper.Map<ItemDetailsDto>(obj, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = flight.Id;
                }),
            CabinBaggageRequiringSeat or ExtraSeat => 
                _mapper.Map<PassengerOrItemOverviewDto>(obj, opt =>
                {
                    opt.Items["DepartureDateTime"] = flight.DepartureDateTime;
                    opt.Items["FlightId"] = flight.Id;
                }),              
            _ => null
        };
        
        return dto;
    }

    public List<object> MapPassengerOrItemsToDto(IEnumerable<object> passengerOrItems, BaseFlight flight,
        bool displayDetails = true)
    {
        return passengerOrItems
            .Select(passengerOrItem => MapSinglePassengerOrItemToDto(passengerOrItem, flight, displayDetails))
            .ToList();
    }
}