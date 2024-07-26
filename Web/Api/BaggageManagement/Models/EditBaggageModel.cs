using System.ComponentModel.DataAnnotations;
using Core.BaggageContext.Enums;

namespace Web.Api.BaggageManagement.Models
{
    public class EditBaggageModel
    {
        [Required]
        public Guid BaggageId { get; }
        
        public int Weight { get; set; }
        
        public SpecialBagEnum? SpecialBagType { get; set; }
        
        public string? Description { get; set; }
    }
}
