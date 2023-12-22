using Core.PassengerContext.JoinClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.PassengerContext.Booking
{
    public class Comment
    {
        public int Id { get; private set; }

        public Passenger Passenger { get; private set; }
        public Guid PassengerId { get; private set; }


        [Required]
        public CommentTypeEnum CommentType { get; private set; }

        [Required]
        [MaxLength(150)]
        public string Text { get; private set; }

        public bool IsMarkedAsRead { get; private set; } = false;
    }
}
