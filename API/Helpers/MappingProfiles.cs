using AutoMapper;
using Core.BaggageContext;
using Core.Dtos;
using Core.FlightContext;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.JoinClasses;
using Core.PassengerContext.Regulatory;
using Core.SeatingContext;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Flight, FlightOverviewDto>()
                .ForMember(d => d.ScheduledFlight, o => o.MapFrom(s => s.ScheduledFlight.FlightNumber))
                .ForMember(d => d.DestinationFrom, o => o.MapFrom(s => s.ScheduledFlight.DestinationFromId))
                .ForMember(d => d.DestinationTo, o => o.MapFrom(s => s.ScheduledFlight.DestinationToId));

            CreateMap<Flight, FlightDetailsDto>()
                .IncludeBase<Flight, FlightOverviewDto>()
                .ForMember(d => d.CodeShare, o => o.MapFrom(s => s.ScheduledFlight.Codeshare))
                .ForMember(d => d.Airline, o => o.MapFrom(s => s.ScheduledFlight.AirlineId));

            CreateMap<Flight, FlightConnectionsDto>()
                .IncludeBase<Flight, FlightDetailsDto>();

            CreateMap<Passenger, PassengerOverviewDto>()
                .ForMember(dest => dest.PNR, opt => opt.MapFrom(src => src.PNR.PNR))
                .ForMember(dest => dest.NumberOfCheckedBags, opt => opt.MapFrom(src => src.PassengerCheckedBags.Count))
                .ForMember(dest => dest.CurrentFlight, o => o.MapFrom((s, d, _, context) => s.Flights
                    .FirstOrDefault(f => f.Flight.DepartureDateTime == (DateTime)context.Items["DepartureDateTime"])))
                .ForMember(dest => dest.SeatOnCurrentFlight, opt => opt.MapFrom(src => src.AssignedSeats))
                .ForMember(dest => dest.SeatOnCurrentFlight, opt => opt.MapFrom((s, d, _, context) => s.AssignedSeats
                    .FirstOrDefault(a => a.FlightId == (int)context.Items["FlightId"])));

            CreateMap<Passenger, PassengerDetailsDto>()
                .IncludeBase<Passenger, PassengerOverviewDto>()
                .ForMember(dest => dest.FrequentFlyer, opt => opt.MapFrom(src => src.FrequentFlyer.FrequentFlyerNumber))
                .ForMember(dest => dest.ConnectingFlights, o => o.MapFrom((s, d, _, context) => s.Flights
                    .Where(f => f.Flight.DepartureDateTime > (DateTime)context.Items["DepartureDateTime"])))
                .ForMember(dest => dest.InboundFlights, o => o.MapFrom((s, d, _, context) => s.Flights
                    .Where(f => f.Flight.DepartureDateTime < (DateTime)context.Items["DepartureDateTime"])))
                .ForMember(dest => dest.OtherFlightsSeats, o => o.MapFrom((s, d, _, context) => s.AssignedSeats
                    .Where(a => a.FlightId != (int)context.Items["FlightId"])));

            CreateMap<PassengerFlight, PassengerFlightDto>()
                .ForMember(d => d.FlightNumber, o => o.MapFrom(s => s.Flight.ScheduledFlight.FlightNumber))
                .ForMember(d => d.DestinationFrom, o => o.MapFrom(s => s.Flight.ScheduledFlight.DestinationFromId))
                .ForMember(d => d.DestinationTo, o => o.MapFrom(s => s.Flight.ScheduledFlight.DestinationToId))
                .ForMember(d => d.DepartureDateTime, o => o.MapFrom(s => s.Flight.DepartureDateTime));
                
            CreateMap<Seat, SeatDto>()
                .ForMember(d => d.PassengerFirstName, o => o.MapFrom(s => s.Passenger.FirstName))
                .ForMember(d => d.PassengerLastName, o => o.MapFrom(s => s.Passenger.LastName));

            CreateMap<APISData, APISDataDto>()
                .ForMember(d => d.IssueCountry, o => o.MapFrom(s => s.IssueCountryId))
                .ForMember(d => d.Nationality, o => o.MapFrom(s => s.NationalityId));

            CreateMap<Baggage, BaggageBaseDto>()
                .ForMember(d => d.TagNumber, o => o.MapFrom(s => s.BaggageTag.TagNumber))
                .ForMember(d => d.FinalDestination, o => o.MapFrom(s => s.FinalDestination.IATAAirportCode))
                .ForMember(d => d.PassengerFirstName, o => o.MapFrom(s => s.Passenger.FirstName))
                .ForMember(d => d.PassengerLastName, o => o.MapFrom(s => s.Passenger.LastName))
                .ForMember(d => d.SpecialBagType, o => o.MapFrom(s => s.SpecialBag.SpecialBagType))
                .ForMember(d => d.SpecialBagDescription, o => o.MapFrom(s => s.SpecialBag.SpecialBagDescription));

            CreateMap<Baggage, BaggageOverviewDto>()
                .IncludeBase<Baggage, BaggageBaseDto>()
                .ForMember(d => d.BaggageType, o => o.MapFrom((s, d, _, context) => s.Flights
                    .FirstOrDefault(f => f.FlightId == (int)context.Items["FlightId"])?.BaggageType));

            CreateMap<Baggage, BaggageDetailsDto>()
                .IncludeBase<Baggage, BaggageBaseDto>();                

            CreateMap<FlightBaggage, FlightBaggageDto>()
                .ForMember(d => d.Flight, o => o.MapFrom(s => s.Flight));

            CreateMap<Comment, CommentDto>();

            CreateMap<SpecialServiceRequest, SpecialServiceRequestDto>()
                .ForMember(d => d.SSRCode, o => o.MapFrom(s => s.SSRCode.Code));
        }
    }
}
