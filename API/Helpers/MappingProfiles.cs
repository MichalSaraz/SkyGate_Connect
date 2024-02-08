using AutoMapper;
using Core.BaggageContext;
using Core.Dtos;
using Core.FlightContext;
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

            CreateMap<Passenger, PassengerOverviewDto>()
                .ForMember(dest => dest.PNR, opt => opt.MapFrom(src => src.PNR.PNR))
                .ForMember(dest => dest.Flights, opt => opt.MapFrom(src => src.Flights))
                .ForMember(dest => dest.AssignedSeats, opt => opt.MapFrom(src => src.AssignedSeats));

            CreateMap<Passenger, PassengerDetailsDto>()
                .IncludeBase<Passenger, PassengerOverviewDto>()
                .ForMember(dest => dest.FrequentFlyer, opt => opt.MapFrom(src => src.FrequentFlyer.FrequentFlyerNumber));
                //.ForMember(dest => dest.TravelDocuments, opt => opt.MapFrom(src => src.TravelDocuments))                    //je nutne?
                //.ForMember(dest => dest.PassengerCheckedBags, opt => opt.MapFrom(src => src.PassengerCheckedBags))          //je nutne?
                //.ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))                                  //je nutne?
                //.ForMember(dest => dest.SpecialServiceRequests, opt => opt.MapFrom(src => src.SpecialServiceRequests));     //je nutne?

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

            CreateMap<Baggage, BaggageOverviewDto>()
                .ForMember(d => d.TagNumber, o => o.MapFrom(s => s.BaggageTag.TagNumber))
                .ForMember(d => d.FinalDestination, o => o.MapFrom(s => s.FinalDestination.IATAAirportCode));

            CreateMap<Baggage, BaggageDetailsDto>()
                .IncludeBase<Baggage, BaggageOverviewDto>()
                .ForMember(d => d.PassengerFirstName, o => o.MapFrom(s => s.Passenger.FirstName))
                .ForMember(d => d.PassengerLastName, o => o.MapFrom(s => s.Passenger.LastName))
                .ForMember(d => d.SpecialBagType, o => o.MapFrom(s => s.SpecialBag.SpecialBagType))
                .ForMember(d => d.SpecialBagDescription, o => o.MapFrom(s => s.SpecialBag.SpecialBagDescription))
                .ForMember(d => d.Flights, o => o.MapFrom(s => s.Flights.Select(f => f.Flight.ScheduledFlightId).ToList()));

            CreateMap<Comment, CommentDto>();

            CreateMap<SpecialServiceRequest, SpecialServiceRequestDto>()
                .ForMember(d => d.SSRCode, o => o.MapFrom(s => s.SSRCode.Code));
        }
    }
}
