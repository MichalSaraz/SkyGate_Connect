using System.ComponentModel.DataAnnotations;

namespace Web.Api.PassengerContext.Models
{
    public abstract class EditAPISDataModel : APISDataModel
    {
        [Required]
        public Guid APISDataId { get; set; }
    }
}
