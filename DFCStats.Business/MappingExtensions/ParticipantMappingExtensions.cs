using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Participants;

namespace DFCStats.Business.MappingExtensions
{
    public static class ParticipantMappingExtensions
    {
        /// <summary>
        /// Maps a Participation entity to a ParticipantsDTO
        /// </summary>
        /// <param name="participation">The participation entity to map</param>
        /// <returns>The mapped ParticipantsDTO, or null if participation is null</returns>
        public static ParticipationDTO? MapToParticipantsDTO(this Participation participation)
        {
            if (participation == null)
                return null;

            return new ParticipationDTO
            {
                Id = participation.Id,
                FixtureId = participation.FixtureId,
                SeasonId = participation.Fixture?.SeasonId,
                OrderNo = participation.OrderNo,
                FirstName = participation.Person?.FirstName,
                LastName = participation.Person?.LastName,
                Role = (participation.Started) ? "Starting XI" : "Substitute",
                Started = participation.Started,
                Sub = participation.Sub,
                YellowCard = participation.YellowCard,
                RedCard = participation.RedCard,
                Goals = participation.Goals,
                ReplacedByPersonId = participation.ReplacedByPersonId,
                ReplacedByFirstName = participation.ReplacedByPerson?.FirstName,
                ReplaceByLastName = participation.ReplacedByPerson?.LastName,
                ReplacedByTime = participation.ReplacedTime,
                TeamAndScore = participation.Fixture?.TeamsAndScores(),
                Season = participation.Fixture?.Season?.Description,
                Date = participation.Fixture?.Date
            };
        }
    }
}