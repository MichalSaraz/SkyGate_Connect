using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos
{
    public class PassengerCommentsDto : BasePassengerDto
    {
        public List<CommentDto> Comments { get; set; } = new();
    }
}
