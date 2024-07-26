using System.ComponentModel.DataAnnotations;
using Core.BaggageContext.Enums;

namespace Web.Api.BaggageManagement.Models
{
    public class AddBaggageModel
    {
        [Required]
        public TagTypeEnum TagType { get; set; }
        
        public int Weight { get; set; }
        
        public SpecialBagEnum? SpecialBagType { get; set; }
        
        public BaggageTypeEnum BaggageType { get; set; } = BaggageTypeEnum.Local;
        
        public string? Description { get; set; }
        
        [Required]
        public required string FinalDestination { get; set; }
        
        public string TagNumber { get; set; } = string.Empty;
    }
}
