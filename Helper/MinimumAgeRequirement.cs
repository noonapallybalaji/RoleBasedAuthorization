using Microsoft.AspNetCore.Authorization;

namespace RoleBasedAuthorization.Helper 
{
    public class MinimumAgeRequirement : Attribute,IAuthorizationRequirement
    {
        public int MinimumAge { get; }

        public MinimumAgeRequirement(int age)
        {
            MinimumAge = age;
        }
    }

    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirthClaim = context.User.FindFirst(c => c.Type == "DateOfBirth");
            if (dateOfBirthClaim == null)
                return Task.CompletedTask;

            var dateOfBirth = Convert.ToDateTime(dateOfBirthClaim.Value);
            var age = DateTime.Today.Year - dateOfBirth.Year;

            if (age >= requirement.MinimumAge)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

}
