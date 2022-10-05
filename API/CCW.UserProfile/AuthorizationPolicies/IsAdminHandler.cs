using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CCW.UserProfile.AuthorizationPolicies;

public class IsAdminHandler : AuthorizationHandler<AdminRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRequirement requirement)
    {
        if (context.User == null || !context.User.Identity.IsAuthenticated || !context.User.HasClaim(c => c.Type == ClaimTypes.Role))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var roles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

        if (roles.Contains("Admin"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
