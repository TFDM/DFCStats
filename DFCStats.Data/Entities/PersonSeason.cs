using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DFCStats.Data.Entities
{
    public class PersonSeason
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("PersonID")]
        public Guid PersonId { get; set; }
        public Person Person { get; set; } = null!; //Used null! to supress warning as this is a bridging entity and will always have a Person

		[ForeignKey("SeasonID")]
        public Guid SeasonId { get; set; }
        public Season Season { get; set; } = null!; //Used null! to supress warning as this is a bridging entity and will always have a Season
	}
}