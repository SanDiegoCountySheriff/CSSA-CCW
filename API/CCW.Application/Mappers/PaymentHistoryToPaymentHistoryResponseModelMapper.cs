using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class PaymentHistoryToPaymentHistoryResponseModelMapper : IMapper<PaymentHistory, PaymentHistoryResponseModel>
{
    public PaymentHistoryResponseModel Map(PaymentHistory source)
    {
        return new PaymentHistoryResponseModel
        {
            PaymentDateTimeUtc = source.PaymentDateTimeUtc,
            PaymentType = source.PaymentType,
            VendorInfo = source.VendorInfo,
            Amount = source.Amount,
            RecordedBy = source.RecordedBy,
            TransactionId = source.TransactionId,
        };
    }
}