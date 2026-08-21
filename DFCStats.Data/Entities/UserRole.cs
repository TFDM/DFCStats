using System.ComponentModel.DataAnnotations.Schema;

namespace DFCStats.Data.Entities
{
    public class UserRole
    {
        public Guid Id { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!; //Used null! to supress warning as this is a bridging entity and will always have a User

		[ForeignKey("RoleId")]
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!; //Used null! to supress warning as this is a bridging entity and will always have a Role
	}
}