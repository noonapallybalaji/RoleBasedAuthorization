using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
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
                Email = model.Email,
                DateOfBirth = model.DateOfBirth
            };



            _context.users.Add(newUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
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



            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.DateOfBirth,user.DateOfBirth.ToString("yyy-MM-dd"))
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            return RedirectToAction("Dashboard", "Home");
            
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
           // Response.Cookies.Delete(".AspNetCore.Cookies");
            Response.Cookies.Delete("CustomAuth");
            //Response.Cookies.Delete(".AspNetCore.CookieAuth");
            //Response.Cookies.Delete(".AspNetCore.Cookies");

            return RedirectToAction("Index", "Home");
        }


        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return BadRequest(); // Handle error

            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            return Json(claims); // Return claims or redirect to a secure page
        }
        public IActionResult AccessDenied()
        {
            ViewBag.Message = "You do not have permission to access this resource.";

            return View();
        }

        private bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var hasher = new PasswordHasher<object>();
            var result = hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
        private string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }
    }
}
