using System.ComponentModel;

namespace Core.PassengerContext.APIS.Enums
{
    public enum DocumentTypeEnum
    {
        [Description("Alien Passport")]
        AlienPassport,

        [Description("Emergency Passport")]
        EmergencyPassport,

        [Description("Normal Passport")]
        NormalPassport,

        [Description("NationalIdCard")]
        NationalIdCard,

        [Description("Visa")]
        Visa,

        [Description("Residence Permit")]
        ResidencePermit
    }
}
