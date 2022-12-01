using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CCW.UserProfile.AuthorizationPolicies;

public class IsProcessorAndAdminHandler : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var roles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

        if (roles.Contains("CCW-SYSTEM-ADMINS-ROLE"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}