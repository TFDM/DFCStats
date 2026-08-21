namespace DFCStats.Web.Models.Users
{
	public class RoleCheckBox
	{
		public Guid RoleId { get; set; }
		public string RoleName { get; set; } = string.Empty;
		public bool IsSelected { get; set; }
	}
}