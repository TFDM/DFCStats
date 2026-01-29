using DFCStats.Domain.Entities;

namespace DFCStats.Data.Interfaces
{
    public interface IClubRepository
    {
        Task<List<Club>> GetAllAsync();
        Task AddAsync(Club club);
    }
}

