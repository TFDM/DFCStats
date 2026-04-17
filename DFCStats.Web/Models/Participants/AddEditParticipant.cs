using System.ComponentModel.DataAnnotations;

namespace DFCStats.Web.Models.Participants
{
    public class AddEditParticipant
    {
        public Guid Id { get; set; }
		public Guid FixtureId { get; set; }
		[Display(Name = "Person")]
		public Guid PersonId { get; set; }
		[Display(Name = "Role in Fixture")]
		public string RoleInFixture { get; set; } = string.Empty;
		[Display(Name = "Goals")]
		public int Goals { get; set; }
		[Display(Name = "Yellow Card")]
		public bool YellowCard { get; set; }
		[Display(Name = "Red Card")]
		public bool RedCard { get; set; }
		[Display(Name = "Replaced By")]
		public Guid? ReplacedByPersonId { get; set; }
		[Display(Name = "Replaced Time")]
		public int? ReplacedTime { get; set; }
    }
}