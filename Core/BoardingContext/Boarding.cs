using Core.BoardingContext.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.BoardingContext
{
    public class Boarding
    {
        [Required]
        public BoardingStatusEnum BoardingStatus { get; private set; } = BoardingStatusEnum.Closed;
    }
}
