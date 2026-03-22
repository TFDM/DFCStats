using DFCStats.Data.Entities;

namespace DFCStats.Business.MappingExtensions
{
    public static class AppearanceMappingExtensions
    {
        
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

    }
}