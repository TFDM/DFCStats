using DFCStats.Data.Entities;
using DFCStats.Domain.DTOs;

namespace DFCStats.Business.MappingExtensions
{
    public static class VenueMappingExtensions
    {
        /// <summary>
        /// Maps a Venue entity to a VenueDTO
        /// </summary>
        /// <param name="venue"></param>
        /// <returns></returns>
        public static VenueDTO? MapToVenueDTO(this Venue venue)
        {
            if (venue == null)
                return null;

            return new VenueDTO
            {
                Id = venue.Id,
                Description = venue.Description,
                ShortDescription = venue.ShortDescription,
                OrderNo = venue.OrderNo
            };
        }
    }
}