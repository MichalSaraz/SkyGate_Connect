using Core.BaggageContext.Enums;

namespace Web.Api.PassengerContext.Models
{
    public class AddBaggageModel
    {
        public TagTypeEnum TagType { get; set; }
        public int Weight { get; set; }
        public SpecialBagEnum? SpecialBagType { get; set; }
        public BaggageTypeEnum BaggageType { get; set; } = BaggageTypeEnum.Local;
        public string Description { get; set; }
        public string FinalDestination { get; set; }
        public string TagNumber { get; set; }
    }
}
