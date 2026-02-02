using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs;

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
                Name = nationality.Name,
                Country = nationality.Country,
                Icon = nationality.Icon,
                IconImage = (nationality.Icon != null) ? $"<img src=\"/images/Flags/{nationality.Icon}\" />" : null
            };
        }
    }
}