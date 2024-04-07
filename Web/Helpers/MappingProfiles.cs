using AutoMapper;
using Core.BaggageContext;
using Core.Dtos;
using Core.FlightContext;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext;
using Core.PassengerContext.APIS;
using Core.PassengerContext.Booking;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext;

namespace Web.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<BaseFlight, FlightOverviewDto>()
                .ForMember(dest => dest.ScheduledFlight,
                    opt => opt.MapFrom(src =>
                        src is Flight ? ((Flight)src).ScheduledFlightId : (src as OtherFlight).FlightNumber))
                .ForMember(dest => dest.DestinationFrom, opt => opt.MapFrom(src => src.DestinationFromId))
                .ForMember(dest => dest.DestinationTo, opt => opt.MapFrom(src => src.DestinationToId));

            CreateMap<BaseFlight, FlightDetailsDto>()
                .IncludeBase<BaseFlight, FlightOverviewDto>()
                .ForMember(dest => dest.CodeShare,
                    opt => opt.MapFrom(src => src is Flight ? ((Flight)src).ScheduledFlight.Codeshare : null))
                .ForMember(dest => dest.Airline, opt => opt.MapFrom(src => src.AirlineId))
                .ForMember(dest => dest.TotalBookedPassengers,
                    opt => opt.MapFrom(src => src.ListOfBookedPassengers.Count));

            CreateMap<BaseFlight, FlightConnectionsDto>().IncludeBase<BaseFlight, FlightDetailsDto>();

            CreateMap<Passenger, PassengerOverviewDto>()
                .ForMember(dest => dest.PNR, opt => opt.MapFrom(src => src.BookingDetails.PNRId))
                .ForMember(dest => dest.NumberOfCheckedBags, opt => opt.MapFrom(src => src.PassengerCheckedBags.Count))
                .ForMember(dest => dest.CurrentFlight,
                    opt => opt.MapFrom((src, _, _, context) => src.Flights.FirstOrDefault(pf =>
                        pf.Flight.DepartureDateTime == (DateTime)context.Items["DepartureDateTime"])))
                .ForMember(dest => dest.SeatOnCurrentFlight, opt => opt.MapFrom(src => src.AssignedSeats))
                .ForMember(dest => dest.SeatOnCurrentFlight,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.AssignedSeats.FirstOrDefault(s => s.FlightId == (Guid)context.Items["FlightId"])));

            CreateMap<Passenger, PassengerDetailsDto>()
                .IncludeBase<Passenger, PassengerOverviewDto>()
                .ForMember(dest => dest.FrequentFlyerNumber,
                    opt => opt.MapFrom(src => src.FrequentFlyerCard.FrequentFlyerNumber))
                .ForMember(dest => dest.ConnectingFlights,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.Flights.Where(pf =>
                                pf.Flight.DepartureDateTime > (DateTime)context.Items["DepartureDateTime"])
                            .OrderBy(pf => pf.Flight.DepartureDateTime)))
                .ForMember(dest => dest.InboundFlights,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.Flights.Where(pf =>
                                pf.Flight.DepartureDateTime < (DateTime)context.Items["DepartureDateTime"])
                            .OrderBy(pf => pf.Flight.DepartureDateTime)))
                .ForMember(dest => dest.OtherFlightsSeats,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.AssignedSeats.Where(s => s.FlightId != (Guid)context.Items["FlightId"])));

            CreateMap<PassengerFlight, PassengerFlightDto>()
                .ForMember(dest => dest.FlightNumber,
                    opt => opt.MapFrom(src =>
                        src.Flight is Flight
                            ? (src.Flight as Flight).ScheduledFlightId
                            : (src.Flight as OtherFlight).FlightNumber))
                .ForMember(dest => dest.DestinationFrom, opt => opt.MapFrom(src => src.Flight.DestinationFromId))
                .ForMember(dest => dest.DestinationTo, opt => opt.MapFrom(src => src.Flight.DestinationToId))
                .ForMember(dest => dest.DepartureDateTime, opt => opt.MapFrom(src => src.Flight.DepartureDateTime))
                .ForMember(dest => dest.ArrivalDateTime, opt => opt.MapFrom(src => src.Flight.ArrivalDateTime));

            CreateMap<Seat, SeatDto>()
                .ForMember(dest => dest.PassengerFirstName, opt => opt.MapFrom(src => src.PassengerOrItem.FirstName))
                .ForMember(dest => dest.PassengerLastName, opt => opt.MapFrom(src => src.PassengerOrItem.LastName));

            CreateMap<APISData, APISDataDto>()
                .ForMember(dest => dest.CountryOfIssue, opt => opt.MapFrom(src => src.CountryOfIssueId))
                .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.NationalityId));

            CreateMap<Baggage, BaggageBaseDto>()
                .ForMember(dest => dest.TagNumber, opt => opt.MapFrom(src => src.BaggageTag.TagNumber))
                .ForMember(dest => dest.FinalDestination,
                    opt => opt.MapFrom(src => src.FinalDestination.IATAAirportCode))
                .ForMember(dest => dest.PassengerFirstName, opt => opt.MapFrom(src => src.Passenger.FirstName))
                .ForMember(dest => dest.PassengerLastName, opt => opt.MapFrom(src => src.Passenger.LastName))
                .ForMember(dest => dest.SpecialBagType, opt => opt.MapFrom(src => src.SpecialBag.SpecialBagType))
                .ForMember(dest => dest.SpecialBagDescription,
                    opt => opt.MapFrom(src => src.SpecialBag.SpecialBagDescription));

            CreateMap<Baggage, BaggageOverviewDto>()
                .IncludeBase<Baggage, BaggageBaseDto>()
                .ForMember(dest => dest.BaggageType,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.Flights.FirstOrDefault(fb => fb.FlightId == (Guid)context.Items["FlightId"])?.BaggageType));

            CreateMap<Baggage, BaggageDetailsDto>().IncludeBase<Baggage, BaggageBaseDto>();

            CreateMap<FlightBaggage, FlightBaggageDto>()
                .ForMember(dest => dest.Flight, opt => opt.MapFrom(src => src.Flight));

            CreateMap<Comment, CommentDto>();

            CreateMap<SpecialServiceRequest, SpecialServiceRequestDto>()
                .ForMember(dest => dest.SSRCode, opt => opt.MapFrom(src => src.SSRCode.Code));

            CreateMap<FlightComment, FlightCommentDto>()
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.Flight.ScheduledFlightId));
        }
    }
}
