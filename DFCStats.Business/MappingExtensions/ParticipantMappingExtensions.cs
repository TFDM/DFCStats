using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Participants;
using DFCStats.Domain.DTOs.Fixtures;

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
                PersonId = participation.PersonId,
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
                ReplacedByLastName = participation.ReplacedByPerson?.LastName,
                ReplacedByTime = participation.ReplacedTime,
                TeamAndScore = participation.Fixture?.TeamsAndScores(),
                Season = participation.Fixture?.Season?.Description,
                Date = participation.Fixture?.Date
            };
        }

        /// <summary>
        /// Maps a Participation entity to a ParticipationFixtureDTO
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
        public static ParticipationFixtureDTO? MapToParticipationFixtureDTO(this Participation participation)
        {
            if (participation == null)
                return null;

            return new ParticipationFixtureDTO
            {
                ParticipationId = participation.Id,
                FixtureId = participation.FixtureId,
                Date = participation.Fixture!.Date,
                TeamsWithScore = participation.Fixture?.TeamsAndScores(),
                Competition = participation.Fixture?.Competition,
                Scoreline = participation.Fixture?.Scoreline(),
                Outcome = participation.Fixture?.Outcome,
                Season = participation.Fixture?.Season?.Description,
                Goals = participation.Goals,
                Role = (participation.Started) ? "Starting XI": "Substitute"
            };
        }
    }
}