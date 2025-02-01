using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthorization.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Role { get; set; }  
        public string PasswordHash { get; set; }
    }
}
