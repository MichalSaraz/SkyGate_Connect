using Core.BaggageContext.Enums;

namespace API.Api.PassengerContext.Models
{
    public class EditBaggageModel
    {
        public Guid BaggageId { get; }
        public int Weight { get; set; }
        public SpecialBagEnum? SpecialBagType { get; set; }
        public string Description { get; set; }

        public EditBaggageModel(Guid baggageId)
        {
            BaggageId = baggageId;
        }
    }
}
