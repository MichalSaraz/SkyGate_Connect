using Core.BaggageContext.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.BaggageContext
{
    public class SpecialBag
    {
        public SpecialBagEnum SpecialBagType { get; set; }

        [MaxLength(150)]
        public string SpecialBagDescription { get; set; }

        public SpecialBag(SpecialBagEnum specialBagType, string specialBagDescription)
        {
            SpecialBagType = specialBagType;

            var descriptionRequiredTypes = new List<SpecialBagEnum>
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
