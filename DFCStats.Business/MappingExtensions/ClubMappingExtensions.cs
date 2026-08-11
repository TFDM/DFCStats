using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Clubs;

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

        /// <summary>
        /// Maps a View_Clubs entity to a ClubDTO
        /// </summary>
        /// <param name="club"></param>
        /// <returns></returns>
        public static ClubDTO? MapToClubDTO(this View_Clubs club)
        {
            if (club == null)
                return null;

            return new ClubDTO
            {
                Id = club.Id,
                Name = club.Name,
                Played = club.Played,
                Won = club.Won,
                Drawn = club.Drawn,
                Lost = club.Lost,
                GoalsFor = club.GoalsFor,
                GoalsAgainst = club.GoalsAgainst
            };
        }
    }
}