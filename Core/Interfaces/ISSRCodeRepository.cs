using Core.PassengerContext.Booking;

namespace Core.Interfaces
{
    public interface ISSRCodeRepository : IGenericRepository<SSRCode>
    {
        /// <summary>
        /// Retrieves an SSRCode from the SSRCodeRepository based on the provided code.
        /// </summary>
        /// <param name="code">The code of the SSRCode to retrieve.</param>
        /// <returns>The SSRCode with the provided code, or null if not found.</returns>
        Task<SSRCode> GetSSRCodeAsync(string code);
    }
}