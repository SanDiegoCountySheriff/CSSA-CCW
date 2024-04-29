using CCW.Common.Models;

namespace CCW.Payment.Services;

public interface ICosmosDbService
{
    Task UpdateApplication(PermitApplication application);
    Task<PermitApplication> GetApplication(string applicationId, string userId);
    Task<PermitApplication> GetAdminApplication(string applicationId);
    Task AddRefundRequest(RefundRequest refundRequest);
    Task<IEnumerable<RefundRequest>> GetAllRefundRequests();
}
