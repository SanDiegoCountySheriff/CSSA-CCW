using CCW.Common.Models;

namespace CCW.Payment.Clients;

public interface IApplicationServiceClient
{
    Task<HttpResponseMessage> UpdateApplicationPaymentHistoryAsync(PaymentHistory paymentHistory, string applicationId, string userId, CancellationToken cancellationToken);
    Task<HttpResponseMessage> UpdateUserApplicationPaymentHistoryAsync(PaymentHistory paymentHistory, string applicationId, CancellationToken cancellationToken);
}
