using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthorization.Models
{
    public class UserRole
    {
        [Key]
        public int UserId { get; set; }
        public int RoleId { get; set; }
        // Navigation property for the related Role
        public Role Role { get; set; }
    }
}
