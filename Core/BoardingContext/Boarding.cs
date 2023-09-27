using Core.FlightContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BoardingContext
{
    public class Boarding
    {
        [Required]
        public BoardingStatusEnum BoardingStatus { get; private set; } = BoardingStatusEnum.Closed;
    }
}
