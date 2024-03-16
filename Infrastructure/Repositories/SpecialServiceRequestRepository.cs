using Core.PassengerContext.JoinClasses;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SpecialServiceRequestRepository : GenericRepository<SpecialServiceRequest>, ISpecialServiceRequestRepository
    {
        public SpecialServiceRequestRepository(AppDbContext context) : base(context)
        {
        }


    }
}
