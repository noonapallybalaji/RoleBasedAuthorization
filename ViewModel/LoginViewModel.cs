using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthorization.ViewModel
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        [Required(ErrorMessage = "password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
