using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class PaymentHistoryToPaymentHistoryResponseModelMapper : IMapper<PaymentHistory, PaymentHistoryResponseModel>
{
    public PaymentHistoryResponseModel Map(PaymentHistory source)
    {
        return new PaymentHistoryResponseModel
        {
            Amount = source.Amount,
            RecordedBy = source.RecordedBy,
            PaymentDateTimeUtc = source.PaymentDateTimeUtc,
            Comments = source.Comments,
        };
    }
}