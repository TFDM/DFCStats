using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs;

namespace DFCStats.Business.MappingExtensions
{
    public static class ClubMappingExtensions
    {
        /// <summary>
        /// Maps a club entity to a ClubDTO
        /// </summary>
        /// <param name="club"></param>
        /// <returns></returns>
        public static ClubDTO? MapToClubDTO(this Club club)
        {
            if (club == null)
                return null;

            return new ClubDTO
            {
                Id = club.Id,
                Name =  club.Name
            };
        }
    }
}