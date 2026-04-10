using System.Diagnostics;
using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Fixtures;
using DFCStats.Domain.DTOs.Participants;

namespace DFCStats.Business.MappingExtensions
{
    public static class FixtureMappingExtensions
    {
        /// <summary>
        /// Maps a Fixture entity to a FixtureDTO
        /// </summary>
        /// <param name="club"></param>
        /// <returns></returns>
        public static FixtureDTO? MapToFixtureDTO(this Fixture fixture)
        {
            if (fixture == null)
                return null;

            return new FixtureDTO
            {
                Id = fixture.Id,
                SeasonId = fixture.SeasonId,
                Season = fixture.Season?.Description,
                Date = fixture.Date,
                ClubId = fixture.ClubId,
                Club = fixture.Club?.Name,
                CategoryId = fixture.CategoryId,
                Category = fixture.Category?.Description,
                Competition = fixture.Competition,
                VenueId = fixture.VenueId,
                Venue = fixture.Venue?.Description,
                VenueShort = fixture.Venue?.ShortDescription,
                Scoreline = Scoreline(fixture),
                Teams = Teams(fixture),
                TeamsAndScores = TeamsAndScores(fixture),
                PenaltiesRequired = PenaltiesRequired(fixture),
                PenaltyScoreline = PenaltyScoreline(fixture),
                PenaltyScoreWithOutcome = PenaltyScoreWithOutomce(fixture),
                Outcome = fixture.Outcome,
                Attendance = fixture.Attendance,
                DarlingtonScore = fixture.DarlingtonScore,
                OppositionScore = fixture.OppositionScore,
                DarlingtonPenaltyScore = fixture.DarlingtonPenaltyScore,
                OppositionPenaltyScore = fixture.OppositionPenaltyScore,
                Notes = fixture.Notes,
                Participants = fixture.Participants?
                    .OrderBy(p => p.OrderNo)
                    .Select(p => p.MapToParticipantsDTO())
                    .OfType<ParticipationDTO>()
                    .ToList()
            };
        }

        /// <summary>
        /// Sets the teams using the venue short description to determine whether Darlington are the home or away team
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        public static string? Teams(this Fixture fixture)
        {
            if (fixture.Venue == null || fixture.Club == null)
                return null;

            if (fixture.Venue?.ShortDescription == "A")
                return $"{fixture.Club!.Name} v Darlington";

            return $"Darlington v {fixture.Club!.Name}";
        }

        /// <summary>
        /// Sets the scoreline
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        public static string Scoreline(this Fixture fixture)
        {
            // Sets the scoreline
            return $"{fixture.DarlingtonScore}-{fixture.OppositionScore}";
        }

        /// <summary>
        /// Sets the team and score using the venue short description to determine whether Darlington are the home or away team
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        public static string? TeamsAndScores(this Fixture fixture)
        {
            if (fixture.Venue == null || fixture.Club == null)
                return null;

            if (fixture.Venue?.ShortDescription == "A")
                return $"{fixture.Club?.Name} {fixture.OppositionScore}-{fixture.DarlingtonScore} Darlington";

            return $"Darlington {fixture.DarlingtonScore}-{fixture.OppositionScore} {fixture.Club?.Name}";
        }

        /// <summary>
        /// Returns true if penalties were required, otherwise false
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        public static bool PenaltiesRequired(this Fixture fixture)
        {
            if (fixture.DarlingtonPenaltyScore != null)
                return true;

            return false;
    }

        /// <summary>
        /// Returns the penalty scoreline
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        public static string? PenaltyScoreline(this Fixture fixture)
        {
            if (fixture.DarlingtonPenaltyScore != null)
                return $"{fixture.DarlingtonPenaltyScore}-{fixture.OppositionPenaltyScore}";
            
            return null;
        }

        /// <summary>
        /// Returns either "Darlington won X-X on penalties", "Opposition Club Name won X-X on penalties" or null
        /// </summary>
        /// <param name="fixture"></param>
        /// <returns></returns>
        public static string? PenaltyScoreWithOutomce(this Fixture fixture)
        {
            var penaltyScoreWithOutcome = "";

            if (fixture.DarlingtonPenaltyScore != null)
            {
				switch (fixture.Outcome)
				{
					case "W":
                        penaltyScoreWithOutcome = string.Format("Darlington won {0}-{1} on penalties", fixture.DarlingtonPenaltyScore, fixture.OppositionPenaltyScore);
						break;
					default:
                        penaltyScoreWithOutcome = string.Format("{0} won on {1}-{2} on penalties", fixture.Club?.Name, fixture.OppositionPenaltyScore, fixture.DarlingtonPenaltyScore);
						break;
				}
                return penaltyScoreWithOutcome;
			}

            return null;
        }

    }
}