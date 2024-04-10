using Core.PassengerContext;

namespace Core.Interfaces
{
    public interface IInfantRepository : IBasePassengerOrItemRepository
    {
        Task<Infant> GetInfantByIdAsync(Guid id);
    }
}