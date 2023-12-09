using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext.Regulatory
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
