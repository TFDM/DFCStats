using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs.Nationalities;

namespace DFCStats.Business.MappingExtensions
{
    public static class NationalityMappingExtensions
    {
        /// <summary>
        /// Maps a nationality entity to a NationalityDTO
        /// </summary>
        /// <param name="nationality"></param>
        /// <returns></returns>
        public static NationalityDTO? MapToNationalityDTO(this Nationality nationality)
        {
            if (nationality == null)
                return null;

            return new NationalityDTO
            {
                Id = nationality.Id,
                Nationality = nationality.Name,
                Country = nationality.Country,
                Icon = nationality.Icon
            };
        }
    }
}