using System.ComponentModel;

namespace Core.PassengerContext.Booking.Enums;

public enum NotTravellingReasonEnum
{
    [Description("Customer Request")]
    CustomerRequest,
    
    [Description("Airline Staff")]
    AirlineStaff,
    
    [Description("Denied Boarding")]
    DeniedBoarding,
    
    [Description("Flight Cancelled")]
    FlightCancelled,
    
    [Description("Flight Delayed")]
    FlightDelayed,
    
    [Description("Medical Reasons")]
    MedicalReasons,
    
    [Description("No Show")]
    NoShow,
    
    [Description("Other")]
    Other,
    
    [Description("Regulatory Requirement Not Met")]
    RegulatoryRequirementNotMet,
    
    [Description("Security Reasons")]
    SecurityReasons  
}