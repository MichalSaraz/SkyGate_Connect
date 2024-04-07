using System.ComponentModel.DataAnnotations;

namespace Web.Api.PassengerManagement.Models
{
    public class EditAPISDataModel : APISDataModel
    {
        [Required]
        public Guid APISDataId { get; set; }
    }
}
