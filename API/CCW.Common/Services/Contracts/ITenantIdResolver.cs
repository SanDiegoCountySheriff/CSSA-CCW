namespace CCW.Common.Services.Contracts;

public interface ITenantIdResolver
{
    string GetTenantId(string referrer);
}
