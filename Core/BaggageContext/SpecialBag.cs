using Core.PassengerContext.Booking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BaggageContext
{
    public class SpecialBag
    {   
        public int Id { get; private set; }

        public SpecialBagEnum SpecialBagType { get; private set; } = SpecialBagEnum.None;


        [MaxLength(150)]
        public string SpecialBagDescription { get; private set; }

        public bool IsDescriptionRequired { get; private set; } = false;
    }
}
