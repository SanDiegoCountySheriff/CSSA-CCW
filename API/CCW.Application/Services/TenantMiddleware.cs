namespace CCW.Application.Services;

public class TenantMiddleware
{
    private readonly RequestDelegate _requestDelegate;

    public TenantMiddleware(RequestDelegate requestDelegate)
    {
        _requestDelegate = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var tenantId = context.User.Claims.Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/identityprovider").Select(c => c.Value.Split("/")[3]).FirstOrDefault();

        if (tenantId == null)
        {
            tenantId = context.User.Claims.Where(c => c.Type == "iss").Select(c => c.Value.Split("/")[3]).FirstOrDefault();
        }

        context.Items["TenantId"] = tenantId != null ? tenantId.Replace("-", "_") : null;

        await _requestDelegate(context);
    }
}

public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}