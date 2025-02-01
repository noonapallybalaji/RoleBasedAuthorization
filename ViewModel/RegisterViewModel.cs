using RoleBasedAuthorization.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoleBasedAuthorization.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "email is required")]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("ConfirmPassword", ErrorMessage = "password is not matching with confirm password")]
        public string? ConfirmPassword { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Role { get; set; }

    }
}
