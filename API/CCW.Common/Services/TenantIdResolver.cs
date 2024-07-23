using CCW.Common.Services.Contracts;

namespace CCW.Common.Services;

public class TenantIdResolver : ITenantIdResolver
{
    private readonly Dictionary<string, string> _tenantIds;

    public TenantIdResolver(Dictionary<string, string> tenantIds)
    {
        _tenantIds = tenantIds;
    }

    public string GetTenantId(string referrer)
    {
        return _tenantIds[referrer]?.Replace("-", "_");
    }
}
