using Core.Interfaces;
using Core.PassengerContext.Booking;

namespace Infrastructure.Repositories
{
    public interface ISSRCodeRepository : IGenericRepository<SSRCode>
    {
        Task<SSRCode> GetSSRCodeAsync(string code);
    }
}