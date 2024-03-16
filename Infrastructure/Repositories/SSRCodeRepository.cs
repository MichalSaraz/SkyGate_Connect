using Core.PassengerContext.Booking;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SSRCodeRepository : GenericRepository<SSRCode>, ISSRCodeRepository
    {
        public SSRCodeRepository(AppDbContext context) : base(context)
        {
        }

        //public async Task<IReadOnlyList<SSRCode>> GetSSRCodesAsync(string code)
        //{
        //    return await _context.SSRCodes.AsNoTracking().ToListAsync();
        //}

        public async Task<SSRCode> GetSSRCodeAsync(string code)
        {
            return await _context.SSRCodes.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code);
        }
    }
}
