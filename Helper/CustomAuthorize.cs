using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using RoleBasedAuthorization.Data;
using System.Security.Claims;

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

                                                                    // Retrieve roles from claims
            var userRoles = user.FindAll(ClaimTypes.Role).Select(claim => claim.Value);

            // Check if the user has the required role
            if (!userRoles.Contains(_role))
            {
                context.Result = new ForbidResult();
            }
        }

    }

}
