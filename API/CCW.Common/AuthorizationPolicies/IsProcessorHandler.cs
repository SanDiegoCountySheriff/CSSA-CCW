using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CCW.Common.AuthorizationPolicies
{
    public class IsProcessorHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var roles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

            if (roles.Contains("CCW-PROCESSORS-ROLE"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
