using Core.PassengerContext.JoinClasses;
using Infrastructure.Data;
using Core.Interfaces;

namespace Infrastructure.Repositories
{
    public class SpecialServiceRequestRepository : GenericRepository<SpecialServiceRequest>,
        ISpecialServiceRequestRepository
    {
        public SpecialServiceRequestRepository(AppDbContext context) : base(context)
        {
        }
    }
}