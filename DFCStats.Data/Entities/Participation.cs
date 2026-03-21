using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DFCStats.Data.Entities
{
    public class Participation
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid PersonId { get; set; }
        [Required]
        public Guid FixtureId { get; set; }
        [Required]
        public int OrderNo { get; set; }
        [Required]
        public bool Started { get; set; }
        [Required]
        public bool Sub { get; set; }
        [Required]
        public int Goals { get; set; }
        [Required]
        public bool YellowCard { get; set; }
        [Required]
        public bool RedCard { get; set; }
        public Guid? ReplacedByPersonId { get; set; }
        public int? ReplacedTime { get; set; }

        public virtual Person? Person { get; set; }
        public virtual Person? ReplacedByPerson { get; set; }
        public virtual Fixture? Fixture { get; set; }
    }
}