namespace DFCStats.Web.Models.Participants
{
    public class Participant
    {
        public Guid Id { get; set; }
		public Guid FixtureId { get; set; }
		public Guid PersonId { get; set; }
		public string LastName { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string RoleInFixture { get; set; } = string.Empty;
		public bool Started { get; set; }
		public bool Substitute { get; set; }
		public int Goals { get; set; }
		public bool YellowCard { get; set; }
		public bool RedCard { get; set; }
		public Guid? ReplacedByPersonId { get; set; }
		public string? ReplacedByFirstName { get; set; }
		public string? ReplacedByLastName { get; set; }
		public int? ReplacedTime { get; set; }
		public int OrderNumber { get; set; }

    }
}