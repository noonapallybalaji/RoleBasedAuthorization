using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoleBasedAuthorization.Data;
using RoleBasedAuthorization.Models;
using RoleBasedAuthorization.ViewModel;
using System.Security.Claims;

namespace RoleBasedAuthorization.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register() 
        {
            return View();
        }

        public  bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var hasher = new PasswordHasher<object>();
            var result = hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
        public string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Hash the password
            string hashedPassword = HashPassword(model.Password);

            // Save the user with hashed password and role to the database
            var newUser = new User
            {
                Username = model.Name,
                PasswordHash = hashedPassword,
                Role = model.Role,// Or save to UserRole table
                Email = model.Email
            };



            _context.users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.Username == model.Username);
            if (user == null )
            {
                ModelState.AddModelError(string.Empty, "User does not exist.");
                return View();
            }

            // Verify the password
            if (!VerifyPassword(user.PasswordHash, model.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            var role = (from use in _context.users
                       join rol in _context.userRoles on use.UserId equals rol.UserId select rol.Role).ToString();


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, "CustomAuthType");
            var principal = new ClaimsPrincipal(identity);

            // Sign in the user (Cookie Authentication as an example)
            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index", "Home");
            
        }

    }
}
