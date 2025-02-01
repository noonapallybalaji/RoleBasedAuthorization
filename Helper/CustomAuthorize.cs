using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using RoleBasedAuthorization.Data;

namespace RoleBasedAuthorization.Helper
{
    public class CustomAuthorizeAttribute(string role) : Attribute, IAuthorizationFilter
    {
        private readonly string _role = role;


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Check if user is authenticated
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Retrieve user's roles from the database
            var userId = int.Parse(user.Identity.Name); // Assuming UserId is stored in Name
            var userRoles = GetUserRoles(userId);

            // Check if the user has the required role
            if (!userRoles.Contains(_role))
            {
                context.Result = new ForbidResult();
            }
        }

        private List<string> GetUserRoles(int userId)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                            .UseSqlServer("Server=COGNINE-L199;Database=PetMarket;Trusted_Connection=True;MultipleActiveResultSets=true;Trust Server Certificate=True")
                            .Options;
            // Replace with your DB access logic
            using (var context = new AppDbContext(options))
            {
                return context.userRoles
                    .Where(ur => ur.UserId == userId)
                    .Select(ur => ur.Role.RoleName)
                    .ToList();
            }

        }
    }

}
