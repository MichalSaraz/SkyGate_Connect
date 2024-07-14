using AutoMapper;
using Core.BaggageContext;
using Core.Dtos;
using Core.FlightContext;
using Core.FlightContext.JoinClasses;
using Core.PassengerContext;
using Core.PassengerContext.APIS;
using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext;
using Core.SeatingContext.Enums;

namespace Web.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Flight mappings
            CreateMap<BaseFlight, FlightOverviewDto>();

            CreateMap<Flight, FlightOverviewDto>()
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.ScheduledFlightId))
                .ForMember(dest => dest.DestinationFrom, opt => opt.MapFrom(src => src.DestinationFromId))
                .ForMember(dest => dest.DestinationTo, opt => opt.MapFrom(src => src.DestinationToId));

            CreateMap<OtherFlight, FlightOverviewDto>()
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.FlightNumber))
                .ForMember(dest => dest.DestinationFrom, opt => opt.MapFrom(src => src.DestinationFromId))
                .ForMember(dest => dest.DestinationTo, opt => opt.MapFrom(src => src.DestinationToId));

            CreateMap<BaseFlight, FlightDetailsDto>();
            
            CreateMap<Flight, FlightDetailsDto>()
                .IncludeBase<Flight, FlightOverviewDto>()
                .ForMember(dest => dest.FlightDuration,
                    opt => opt.MapFrom(src =>
                        src.ScheduledFlight.FlightDuration
                            .FirstOrDefault(kvp => kvp.Key == src.DepartureDateTime.DayOfWeek)
                            .Value))
                .ForMember(dest => dest.CodeShare, opt => opt.MapFrom(src => src.ScheduledFlight.Codeshare))
                .ForMember(dest => dest.ArrivalAirportName, opt => opt.MapFrom(src => src.DestinationTo.AirportName))
                .ForMember(dest => dest.AircraftRegistration, opt => opt.MapFrom(src => src.AircraftId))
                .ForMember(dest => dest.AircraftType, opt => opt.MapFrom(src => src.Aircraft.AircraftTypeId))
                .ForMember(dest => dest.TotalBookedInfants,
                    opt => opt.MapFrom(src => src.ListOfBookedPassengers.Count(pf => pf.PassengerOrItem is Infant)))
                .ForMember(dest => dest.TotalCheckedInInfants,
                    opt => opt.MapFrom(src => src.ListOfBookedPassengers.Count(pf =>
                        pf.PassengerOrItem is Infant && pf.AcceptanceStatus == AcceptanceStatusEnum.Accepted)))
                .ForMember(dest => dest.BookedPassengers,
                    opt => opt.MapFrom(src =>
                        src.ListOfBookedPassengers.GroupBy(pf => pf.FlightClass)
                            .ToDictionary(g => g.Key, g => g.Count())))
                .ForMember(dest => dest.StandbyPassengers,
                    opt => opt.MapFrom(src =>
                        src.ListOfBookedPassengers.GroupBy(pf => pf.FlightClass)
                            .ToDictionary(g => g.Key,
                                g => g.Count(pf => pf.AcceptanceStatus == AcceptanceStatusEnum.Standby))))
                .ForMember(dest => dest.CheckedInPassengers,
                    opt => opt.MapFrom(src =>
                        src.ListOfBookedPassengers.GroupBy(pf => pf.FlightClass)
                            .ToDictionary(g => g.Key,
                                g => g.Count(pf => pf.AcceptanceStatus == AcceptanceStatusEnum.Accepted))))
                .ForMember(dest => dest.AircraftConfiguration,
                    opt => opt.MapFrom(src =>
                        src.Seats.GroupBy(s => s.FlightClass).ToDictionary(g => g.Key, g => g.Count())))
                .ForMember(dest => dest.CabinCapacity,
                    opt => opt.MapFrom(src =>
                        src.Seats.GroupBy(s => s.FlightClass)
                            .ToDictionary(g => g.Key, g => g.Count(c => c.SeatStatus != SeatStatusEnum.Inop))))
                .ForMember(dest => dest.AvailableSeats,
                    opt => opt.MapFrom(src =>
                        src.Seats.GroupBy(s => s.FlightClass)
                            .ToDictionary(g => g.Key, g => g.Count(c => c.SeatStatus == SeatStatusEnum.Empty))));

            CreateMap<BaseFlight, FlightConnectionsDto>()
                .IncludeBase<BaseFlight, FlightOverviewDto>()
                .Include<Flight, FlightConnectionsDto>()
                .Include<OtherFlight, FlightConnectionsDto>();
            
            CreateMap<Flight, FlightConnectionsDto>()
                .IncludeBase<Flight, FlightOverviewDto>();
            
            CreateMap<OtherFlight, FlightConnectionsDto>()
                .IncludeBase<OtherFlight, FlightOverviewDto>();

            
            // Baggage mappings
            CreateMap<Baggage, BaggageOverviewDto>()
                .ForMember(dest => dest.TagNumber, opt => opt.MapFrom(src => src.BaggageTag.TagNumber))
                .ForMember(dest => dest.FinalDestination, opt => opt.MapFrom(src => src.DestinationId))
                .ForMember(dest => dest.PassengerFirstName, opt => opt.MapFrom(src => src.Passenger.FirstName))
                .ForMember(dest => dest.PassengerLastName, opt => opt.MapFrom(src => src.Passenger.LastName))
                .ForMember(dest => dest.SpecialBagType, opt => opt.MapFrom(src => src.SpecialBag.SpecialBagType))
                .ForMember(dest => dest.SpecialBagDescription,
                    opt => opt.MapFrom(src => src.SpecialBag.SpecialBagDescription))
                .ForMember(dest => dest.BaggageType,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.Flights?.FirstOrDefault(fb => fb.FlightId == (Guid)context.Items["FlightId"])
                            ?.BaggageType));

            CreateMap<Baggage, BaggageDetailsDto>().IncludeBase<Baggage, BaggageOverviewDto>();

            
            // BasePassengerOrItem mapping
            CreateMap<BasePassengerOrItem, BasePassengerOrItemDto>()
                .ForMember(dest => dest.SeatNumberOnCurrentFlight,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.AssignedSeats?.FirstOrDefault(s => s.FlightId == (Guid)context.Items["FlightId"])
                            ?.SeatNumber))
                .ForMember(dest => dest.PNR, opt => opt.MapFrom(src => src.BookingDetails.PNRId))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src =>
                        src is Passenger ? "Passenger" :
                        src is ExtraSeat ? "ExtraSeat" :
                        src is CabinBaggageRequiringSeat ? "CabinBaggageRequiringSeat" :
                        src is Infant ? "Infant" : null));

            CreateMap<BasePassengerOrItem, PassengerOrItemOverviewDto>()
                .IncludeBase<BasePassengerOrItem, BasePassengerOrItemDto>()
                .ForMember(dest => dest.CurrentFlight,
                    opt => opt.MapFrom((src, _, _, context) => src.Flights?.FirstOrDefault(pf =>
                        pf.Flight?.DepartureDateTime == (DateTime)context.Items["DepartureDateTime"])))
                .ForMember(dest => dest.SeatOnCurrentFlightDetails, opt => opt.MapFrom(src => src.AssignedSeats))
                .ForMember(dest => dest.SeatOnCurrentFlightDetails,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.AssignedSeats?.FirstOrDefault(s => s.FlightId == (Guid)context.Items["FlightId"])))
                .ForMember(dest => dest.ConnectingFlights,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.Flights.Where(pf =>
                                pf.Flight?.DepartureDateTime > (DateTime)context.Items["DepartureDateTime"])
                            .OrderBy(pf => pf.Flight?.DepartureDateTime)))
                .ForMember(dest => dest.InboundFlights,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.Flights?.Where(pf =>
                                pf.Flight?.DepartureDateTime < (DateTime)context.Items["DepartureDateTime"])
                            .OrderBy(pf => pf.Flight?.DepartureDateTime)))
                .ForMember(dest => dest.OtherFlightsSeats,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.AssignedSeats?.Where(s => s.FlightId != (Guid)context.Items["FlightId"])));

            
            // Passenger mappings
            CreateMap<Passenger, BasePassengerOrItemDto>()
                .IncludeBase<BasePassengerOrItem, BasePassengerOrItemDto>();

            CreateMap<Passenger, PassengerOrItemOverviewDto>()
                .IncludeBase<BasePassengerOrItem, PassengerOrItemOverviewDto>()
                .ForMember(dest => dest.NumberOfCheckedBags, opt => opt.MapFrom(src => src.PassengerCheckedBags.Count));
            
            CreateMap<Passenger, PassengerDetailsDto>()
                .IncludeBase<Passenger, PassengerOrItemOverviewDto>()
                .ForMember(dest => dest.FrequentFlyerNumber,
                    opt => opt.MapFrom(src => src.FrequentFlyerCard.FrequentFlyerNumber));
                

            // ExtraSeat mappings
            CreateMap<ExtraSeat, BasePassengerOrItemDto>()
                .IncludeBase<BasePassengerOrItem, BasePassengerOrItemDto>();

            CreateMap<ExtraSeat, PassengerOrItemOverviewDto>()
                .IncludeBase<BasePassengerOrItem, PassengerOrItemOverviewDto>();
            
            CreateMap<ExtraSeat, ItemDetailsDto>()
                .IncludeBase<ExtraSeat, PassengerOrItemOverviewDto>();

            
            // CabinBaggageRequiringSeat mappings
            CreateMap<CabinBaggageRequiringSeat, BasePassengerOrItemDto>()
                .IncludeBase<BasePassengerOrItem, BasePassengerOrItemDto>();

            CreateMap<CabinBaggageRequiringSeat, PassengerOrItemOverviewDto>()
                .IncludeBase<BasePassengerOrItem, PassengerOrItemOverviewDto>();
            
            CreateMap<CabinBaggageRequiringSeat, ItemDetailsDto>()
                .IncludeBase<CabinBaggageRequiringSeat, PassengerOrItemOverviewDto>();
            

            // Infant mapping
            CreateMap<Infant, BasePassengerOrItemDto>()
                .IncludeBase<BasePassengerOrItem, BasePassengerOrItemDto>();
            
            CreateMap<Infant, InfantOverviewDto>()
                .IncludeBase<Infant, BasePassengerOrItemDto>()
                .ForMember(dest => dest.SeatNumberOnCurrentFlight, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentFlight,
                    opt => opt.MapFrom((src, _, _, context) => src.Flights?.FirstOrDefault(pf =>
                        pf.Flight?.DepartureDateTime == (DateTime)context.Items["DepartureDateTime"])));

            CreateMap<Infant, InfantDetailsDto>()
                .IncludeBase<Infant, InfantOverviewDto>()
                .ForMember(dest => dest.ConnectingFlights,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.Flights?.Where(pf =>
                                pf.Flight?.DepartureDateTime > (DateTime)context.Items["DepartureDateTime"])
                            .OrderBy(pf => pf.Flight?.DepartureDateTime)))
                .ForMember(dest => dest.InboundFlights,
                    opt => opt.MapFrom((src, _, _, context) =>
                        src.Flights?.Where(pf =>
                                pf.Flight?.DepartureDateTime < (DateTime)context.Items["DepartureDateTime"])
                            .OrderBy(pf => pf.Flight?.DepartureDateTime)));

            
            // Seat mapping
            CreateMap<Seat, SeatDto>()
                .ForMember(dest => dest.PassengerFirstName, opt => opt.MapFrom(src => src.PassengerOrItem.FirstName))
                .ForMember(dest => dest.PassengerLastName, opt => opt.MapFrom(src => src.PassengerOrItem.LastName))
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.Flight.ScheduledFlightId));

            
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
                .ForMember(dest => dest.Flight, opt => opt.UseDestinationValue());

            CreateMap<FlightComment, FlightCommentDto>()
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.Flight.ScheduledFlightId));

            CreateMap<SpecialServiceRequest, SpecialServiceRequestDto>()
                .ForMember(dest => dest.SSRCode, opt => opt.MapFrom(src => src.SSRCodeId))
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.Flight.ScheduledFlightId));

            CreateMap<BasePassengerOrItem, PassengerOrItemCommentsDto>()
                .IncludeBase<BasePassengerOrItem, BasePassengerOrItemDto>();

            CreateMap<BasePassengerOrItem, PassengerSpecialServiceRequestsDto>()
                .IncludeBase<BasePassengerOrItem, BasePassengerOrItemDto>();
        }
    }
}