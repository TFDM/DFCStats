using System.Linq.Expressions;
using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Appearances;

namespace DFCStats.Business.MappingExtensions
{
    public static class AppearanceMappingExtensions
    {
        /// <summary>
        /// Maps a list of participation entities to a list of SeasonAppearanceDTO
        /// </summary>
        /// <param name="participations"></param>
        /// <returns></returns>
        public static List<SeasonalAppearanceDTO>? MapToAppearanceDTO(this List<Participation> participations)
        {
            if (participations == null)
                return null;

            List<SeasonalAppearanceDTO> appearances = new List<SeasonalAppearanceDTO>();

            // Loops over each distinct season in the participation records
            foreach (var season in participations.OrderBy(p => p.Fixture?.Season?.Description).Select(p => p.Fixture!.Season).DistinctBy(s => s?.Id))
            {
                // Pre-filter the participations for THIS specific season once to keep it clean
                var seasonParticipations = participations.Where(p => p.Fixture?.SeasonId == season?.Id).ToList();

                // For each season add the SeasonAppearanceDTO 
                appearances.Add(new SeasonalAppearanceDTO
                {
                    PersonId = participations.First().PersonId,
                    FirstName = participations.First().Person?.FirstName,
                    LastName = participations.First().Person?.LastName,
                    SeasonId = season?.Id,
                    SeasonDescription = season?.Description,
                    TotalAppearances = seasonParticipations.TotalAppearances(),
                    Goals = seasonParticipations.TotalGoals(),
                    Starts = seasonParticipations.TotalStarts(),
                    Subs = seasonParticipations.TotalSubs(),
                    RedCards = seasonParticipations.TotalRedCards(),
                    LeagueStarts = seasonParticipations.TotalLeagueStarts(),
                    LeagueSubs = seasonParticipations.TotalLeagueSubs(),
                    LeagueGoals = seasonParticipations.TotalLeagueGoals(),
                    CupStarts = seasonParticipations.TotalCupStarts(),
                    CupSubs = seasonParticipations.TotalCupSubs(),
                    CupGoals = seasonParticipations.TotalCupGoals(),
                    PlayOffStarts = seasonParticipations.TotalPlayOffStarts(),
                    PlayOffSubs = seasonParticipations.TotalPlayOffSubs(),
                    PlayOffGoals = seasonParticipations.TotalPlayOffGoals()
                    // RedCards = participations.Where(p => p.Fixture?.SeasonId == season?.Id && p.RedCard == true).Count(),
                    // TotalAppearances = participations.Where(p => p.Fixture?.SeasonId == season?.Id).Count(),
                    // Starts = participations.Where(p => p.Fixture?.SeasonId == season?.Id && p.Started == true).Count(),
                    // Subs = participations.Where(p => p.Fixture?.SeasonId == season?.Id && p.Sub == true).Count(),
                    // Goals = participations.Where(p => p.Fixture?.SeasonId == season?.Id).Sum(p => p.Goals),
                    // LeagueStarts = participations.Where(p => p.Fixture?.SeasonId == season?.Id
                    //     && p.Started == true 
                    //     && p.Fixture?.Category?.PlayOff == false 
                    //     && (p.Fixture?.Category?.FootballLeague == true 
                    //     || p.Fixture?.Category?.NonLeague == true)).Count(),
                    // LeagueSubs = participations.Where(p => p.Fixture?.SeasonId == season?.Id
                    //     && p.Sub == true 
                    //     && p.Fixture?.Category?.PlayOff == false 
                    //     && (p.Fixture?.Category?.FootballLeague == true 
                    //     || p.Fixture?.Category?.NonLeague == true)).Count(),
                    // LeagueGoals = participations.Where(p => p.Fixture?.SeasonId == season?.Id
                    //     && p.Fixture?.Category?.PlayOff == false
                    //     && (p.Fixture?.Category?.FootballLeague == true 
                    //     || p.Fixture?.Category?.NonLeague == true)).Sum(p => p.Goals),
                    // CupStarts = participations.Where(p => p.Fixture?.Season?.Id == season?.Id
                    //     && p.Started == true
                    //     && p.Fixture?.Category?.Cup == true).Count(),
                    // CupSubs = participations.Where(p => p.Fixture?.Season?.Id == season?.Id
                    //     && p.Sub == true
                    //     && p.Fixture?.Category?.Cup == true).Count(),
                    // CupGoals = participations.Where(p => p.Fixture?.Season?.Id == season?.Id
                    //     && p.Fixture?.Category?.Cup == true).Sum(p => p.Goals),
                    // PlayOffStarts = participations.Where(p => p.Fixture?.Season?.Id == season?.Id
                    //     && p.Started == true
                    //     && p.Fixture?.Category?.PlayOff == true).Count(),
                    // PlayOffSubs = participations.Where(p => p.Fixture?.Season?.Id == season?.Id
                    //     && p.Sub == true
                    //     && p.Fixture?.Category?.PlayOff == true).Count(),
                    // PlayOffGoals = participations.Where(p => p.Fixture?.Season?.Id == season?.Id
                    //     && p.Fixture?.Category?.PlayOff == true).Sum(p => p.Goals)
                });
            };

            return appearances;
        }

        /// <summary>
        /// Uses a list of participation records to work out the total appearances
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
        public static int TotalAppearances(this List<Participation> participation)
        {
            return participation.Where(p => p.Fixture?.Category?.Cup == true 
                || p.Fixture?.Category?.FootballLeague == true
                || p.Fixture?.Category?.NonLeague == true
                || p.Fixture?.Category?.PlayOff == true).Count();
        }

        /// <summary>
        /// Uses a list of participation records to work out the total number of goals
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
        public static int TotalGoals(this List<Participation> participation)
        {
            return participation.Where(p => p.Fixture?.Category?.Cup == true 
                || p.Fixture?.Category?.FootballLeague == true
                || p.Fixture?.Category?.NonLeague == true
                || p.Fixture?.Category?.PlayOff == true).Sum(p => p.Goals);
        }

        /// <summary>
        /// Uses a list of participation recrds to work out the total number of red cards
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
        public static int TotalRedCards(this List<Participation> participation)
        {
            return participation.Where(p => p.RedCard == true).Count();
        }

        /// <summary>
        /// Uses a list of participation records to work out the total number of starts in all competitions
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
        public static int TotalStarts(this List<Participation> participation)
        {
            return participation.Where(p => p.Started == true).Count();
        }

        /// <summary>
        /// uses a list of participation records to work out the total number of substitute appearances in all competitions 
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
        public static int TotalSubs(this List<Participation> participation)
        {
            return participation.Where(p => p.Sub == true).Count();
        }

        /// <summary>
        /// Uses a list of participation records to work out the total number of league starts
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
        public static int TotalLeagueStarts(this List<Participation> participation)
        {
            return participation.Where(p => p.Started == true 
                && p.Fixture?.Category?.PlayOff == false 
                && (p.Fixture?.Category?.FootballLeague == true 
                || p.Fixture?.Category?.NonLeague == true)).Count();
        }

        /// <summary>
        /// Uses a list of participation records to work out the total number of league sub appearances
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
        public static int TotalLeagueSubs(this List<Participation> participation)
        {
            return participation.Where(p => p.Sub == true 
                && p.Fixture?.Category?.PlayOff == false 
                && (p.Fixture?.Category?.FootballLeague == true 
                || p.Fixture?.Category?.NonLeague == true)).Count();
        }

        /// <summary>
        /// Uses a list of participation records to work out the total number of league goals
        /// </summary>
        /// <param name="participation"></param>
        /// <returns></returns>
        public static int TotalLeagueGoals(this List<Participation> participation)
        {
            return participation.Where(p => p.Fixture?.Category?.PlayOff == false
                && (p.Fixture?.Category?.FootballLeague == true 
                || p.Fixture?.Category?.NonLeague == true)).Sum(p => p.Goals);
        }

        public static int TotalPlayOffStarts(this List<Participation> participation)
        {
            return participation.Where(p => p.Started == true
                && p.Fixture?.Category?.PlayOff == true).Count();
                    
        }

        public static int TotalPlayOffSubs(this List<Participation> participation)
        {
            return participation.Where(p => p.Sub == true
                && p.Fixture?.Category?.PlayOff == true).Count();
                    
        }

        public static int TotalPlayOffGoals(this List<Participation> participation)
        {
            return participation.Where(p => p.Fixture?.Category?.PlayOff == true).Sum(p => p.Goals);
        }

        public static int TotalCupStarts(this List<Participation> participation)
        {
            return participation.Where(p => p.Started == true
                && p.Fixture?.Category?.Cup == true).Count();

        }

        public static int TotalCupSubs(this List<Participation> participation)
        {
            return participation.Where(p => p.Sub == true
                && p.Fixture?.Category?.Cup == true).Count();
        }

        public static int TotalCupGoals(this List<Participation> participation)
        {
            return participation.Where(p => p.Fixture?.Category?.Cup == true).Sum(p => p.Goals);
        }

    }
}