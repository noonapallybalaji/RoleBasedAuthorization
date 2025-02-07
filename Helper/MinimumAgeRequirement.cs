using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace RoleBasedAuthorization.Helper 
{
    public class MinimumAgeRequirement : Attribute,IAuthorizationRequirement
    {
        public int MinimumAge { get; }
        public int MaximumAge { get; }

        public MinimumAgeRequirement(int minAge, int maxAge )
        {
            MinimumAge = minAge;
            MaximumAge = maxAge;
        }
    }

    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirthClaim = context.User.FindFirst(ClaimTypes.DateOfBirth);
            if (dateOfBirthClaim == null)
                return Task.CompletedTask;

            if (DateTime.TryParse(dateOfBirthClaim.Value, out var dateOfBirth))
            {
                var age = DateTime.Today.Year - dateOfBirth.Year;

                if (age >= requirement.MinimumAge && age < requirement.MaximumAge)
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;

        }
    }

}
