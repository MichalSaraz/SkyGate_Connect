using System.ComponentModel.DataAnnotations;

namespace Web.Api.PassengerContext.Models
{
    public class EditAPISDataModel : APISDataModel
    {
        [Required]
        public Guid APISDataId { get; set; }
    }
}
