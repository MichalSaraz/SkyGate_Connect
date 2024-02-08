using Core.BaggageContext.Enums;
using Core.PassengerContext.Booking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BaggageContext
{
    public class SpecialBag
    {
        public int Id { get; private set; }

        public SpecialBagEnum SpecialBagType { get; set; }

        [MaxLength(150)]
        public string SpecialBagDescription { get; set; }

        public SpecialBag(SpecialBagEnum specialBagType, string specialBagDescription)
        {
            SpecialBagType = specialBagType;

            List<SpecialBagEnum> descriptionRequiredTypes = new List<SpecialBagEnum>
            {
                SpecialBagEnum.AVIH,
                SpecialBagEnum.Firearm,
                SpecialBagEnum.WCLB,
                SpecialBagEnum.WCMP,
                SpecialBagEnum.WCBD,
                SpecialBagEnum.WCBW
            };

            if (descriptionRequiredTypes.Contains(specialBagType) && string.IsNullOrEmpty(specialBagDescription))
            {
                throw new ArgumentException("Description is required");
            }

            SpecialBagDescription = specialBagDescription;
        }
    }
}
