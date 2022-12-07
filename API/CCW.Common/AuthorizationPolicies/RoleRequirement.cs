using Microsoft.AspNetCore.Authorization;


namespace CCW.Common.AuthorizationPolicies
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        protected string Role { get; set; }

        public RoleRequirement(string role)
        {
            Role = role;
        }
    }
}
