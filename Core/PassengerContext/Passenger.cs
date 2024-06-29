using Core.BaggageContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Core.PassengerContext.JoinClasses;

namespace Core.PassengerContext
{
    public class Passenger : BasePassengerOrItem
    {
        public FrequentFlyer FrequentFlyerCard { get; set; }
        public Guid? FrequentFlyerCardId { get; set; }       

        // The 'InfantId' property may have a value only if the associated 'Passenger' instance,
        // which it refers to, has a value in the 'BookingDetails.Age' property greater than or equal to 18.
        public Guid? InfantId { get; set; }
        public Infant Infant { get; set; }

        public int BaggageAllowance { get; set; }
        public bool PriorityBoarding { get; set; }

        public List<Baggage> PassengerCheckedBags { get; set; } = new();        
        public List<SpecialServiceRequest> SpecialServiceRequests { get; set; } = new();
        
        public Passenger(int baggageAllowance, bool priorityBoarding, string firstName, string lastName,
            PaxGenderEnum gender, Guid? bookingDetailsId, int? weight) : base(firstName, lastName, gender,
            bookingDetailsId, weight)
        {
            BaggageAllowance = baggageAllowance;
            PriorityBoarding = priorityBoarding;
        }
    }
}
