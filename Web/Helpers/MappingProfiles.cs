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
            // Flight mappings
            CreateMap<BaseFlight, FlightOverviewDto>()
                .ForMember(dest => dest.ScheduledFlight,
                    opt => opt.MapFrom(src =>
                        src is Flight ? ((Flight)src).ScheduledFlightId :
                        src is OtherFlight ? ((OtherFlight)src).FlightNumber : null))
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
            
            
            // Baggage mappings
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

            
            // BasePassengerOrItem mapping
            CreateMap<BasePassengerOrItem, BasePassengerOrItemDto>()
                .ForMember(dest => dest.SeatNumberOnCurrentFlight,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.AssignedSeats.FirstOrDefault(s => s.FlightId == (Guid)context.Items["FlightId"])
                            ?.SeatNumber))
                .ForMember(dest => dest.PNR, opt => opt.MapFrom(src => src.BookingDetails.PNRId));

            
            // Passenger mappings
            CreateMap<Passenger, PassengerOverviewDto>()
                .IncludeBase<BasePassengerOrItem, BasePassengerOrItemDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "Passenger"))
                .ForMember(dest => dest.NumberOfCheckedBags, opt => opt.MapFrom(src => src.PassengerCheckedBags.Count))
                .ForMember(dest => dest.CurrentFlight,
                    opt => opt.MapFrom((src, _, _, context) => src.Flights.FirstOrDefault(pf =>
                        pf.Flight.DepartureDateTime == (DateTime)context.Items["DepartureDateTime"])))
                .ForMember(dest => dest.SeatOnCurrentFlightDetails, opt => opt.MapFrom(src => src.AssignedSeats))
                .ForMember(dest => dest.SeatOnCurrentFlightDetails,
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

            
            // ExtraSeat mappings
            CreateMap<ExtraSeat, ItemOverviewDto>()
                .IncludeBase<BasePassengerOrItem, BasePassengerOrItemDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "ExtraSeat"))
                .ForMember(dest => dest.CurrentFlight,
                    opt => opt.MapFrom((src, _, _, context) => src.Flights.FirstOrDefault(pf =>
                        pf.Flight.DepartureDateTime == (DateTime)context.Items["DepartureDateTime"])))
                .ForMember(dest => dest.SeatOnCurrentFlightDetails, opt => opt.MapFrom(src => src.AssignedSeats))
                .ForMember(dest => dest.SeatOnCurrentFlightDetails,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.AssignedSeats.FirstOrDefault(s => s.FlightId == (Guid)context.Items["FlightId"])));

            CreateMap<ExtraSeat, ItemDetailsDto>()
                .IncludeBase<ExtraSeat, ItemOverviewDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "ExtraSeat"))
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

            
            // CabinBaggageRequiringSeat mappings
            CreateMap<CabinBaggageRequiringSeat, ItemOverviewDto>()
                .IncludeBase<BasePassengerOrItem, BasePassengerOrItemDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "CabinBaggageRequiringSeat"))
                .ForMember(dest => dest.CurrentFlight,
                    opt => opt.MapFrom((src, _, _, context) => src.Flights.FirstOrDefault(pf =>
                        pf.Flight.DepartureDateTime == (DateTime)context.Items["DepartureDateTime"])))
                .ForMember(dest => dest.SeatOnCurrentFlightDetails, opt => opt.MapFrom(src => src.AssignedSeats))
                .ForMember(dest => dest.SeatOnCurrentFlightDetails,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.AssignedSeats.FirstOrDefault(s => s.FlightId == (Guid)context.Items["FlightId"])));
            
            CreateMap<CabinBaggageRequiringSeat, ItemDetailsDto>()
                .IncludeBase<CabinBaggageRequiringSeat, ItemOverviewDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "CabinBaggageRequiringSeat"))
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

            
            // Infant mapping
            CreateMap<Infant, InfantDto>()
                .IncludeBase<BasePassengerOrItem, BasePassengerOrItemDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => "Infant"));

            
            // Seat mapping
            CreateMap<Seat, SeatDto>()
                .ForMember(dest => dest.PassengerFirstName, opt => opt.MapFrom(src => src.PassengerOrItem.FirstName))
                .ForMember(dest => dest.PassengerLastName, opt => opt.MapFrom(src => src.PassengerOrItem.LastName));

            
            // APISData mapping
            CreateMap<APISData, APISDataDto>()
                .ForMember(dest => dest.CountryOfIssue, opt => opt.MapFrom(src => src.CountryOfIssueId))
                .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.NationalityId));
            
            
            // Comment mapping
            CreateMap<Comment, CommentDto>();
            
            
            // Join classes mappings
            CreateMap<PassengerFlight, PassengerFlightDto>()
                .ForMember(dest => dest.FlightNumber,
                    opt => opt.MapFrom(src =>
                        src.Flight is Flight
                            ? ((Flight)src.Flight).ScheduledFlightId
                            : (src.Flight is OtherFlight ? ((OtherFlight)src.Flight).FlightNumber : null)))
                .ForMember(dest => dest.DestinationFrom, opt => opt.MapFrom(src => src.Flight.DestinationFromId))
                .ForMember(dest => dest.DestinationTo, opt => opt.MapFrom(src => src.Flight.DestinationToId))
                .ForMember(dest => dest.DepartureDateTime, opt => opt.MapFrom(src => src.Flight.DepartureDateTime))
                .ForMember(dest => dest.ArrivalDateTime, opt => opt.MapFrom(src => src.Flight.ArrivalDateTime));
            
            CreateMap<FlightBaggage, FlightBaggageDto>()
                .ForMember(dest => dest.Flight, opt => opt.MapFrom(src => src.Flight));
            
            CreateMap<FlightComment, FlightCommentDto>()
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.Flight.ScheduledFlightId));
            
            CreateMap<SpecialServiceRequest, SpecialServiceRequestDto>()
                .ForMember(dest => dest.SSRCode, opt => opt.MapFrom(src => src.SSRCode.Code));

            

            CreateMap<Passenger, PassengerOrItemCommentsDto>()
                .IncludeBase<Passenger, BasePassengerOrItemDto>();

            CreateMap<Passenger, PassengerSpecialServiceRequestsDto>()
                .IncludeBase<Passenger, BasePassengerOrItemDto>();

            
        }
    }
}
