using DFCStats.Business.Interfaces;
using DFCStats.Data.Interfaces;
using DFCStats.Domain.DTOs;

namespace DFCStats.Business
{
    public class ClubService : IClubService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClubService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ClubDTO>> GetAllClubsAsync()
        {
            var clubs = await _unitOfWork.ClubRepository.GetAllAsync();

            return clubs.Select(c => new ClubDTO
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        public async Task AddClub(ClubDTO dto)
        {
            var club = new DFCStats.Domain.Entities.Club
            {
                Name = dto.Name
            };

            await _unitOfWork.ClubRepository.AddAsync(club);
            await _unitOfWork.CommitChanges();
        }
    }
}