namespace DFCStats.Business.Interfaces
{
    public interface ISeasonService
    {
        /// <summary>
        /// Check to see if a season description is already in use 
        /// Will return true if it is in use otherwise false
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<bool> IsSeasonDescriptionInUse(string description);
    }
}