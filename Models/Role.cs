using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthorization.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        // Navigation property for related UserRoles
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
