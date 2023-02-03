using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class RequestPermitApplicationToPaymentHistoryMapper : IMapper<PermitApplicationRequestModel, PaymentHistory[]>
{
    public PaymentHistory[] Map(PermitApplicationRequestModel source)
    {
        if (source.PaymentHistory != null)
        {
            int count = source.History.Length;
            var newItem = new PaymentHistory[count];
            for (int i = 0; i < count; i++)
            {
                newItem[i] = MapAlias(source.PaymentHistory[i], new PaymentHistory());
            }

            return newItem;
        }

        return Array.Empty<PaymentHistory>();
    }

    private static PaymentHistory MapAlias(PaymentHistory uiPaymentHistory, PaymentHistory dbPaymentHistory)
    {
        dbPaymentHistory.Amount = uiPaymentHistory.Amount;
        dbPaymentHistory.PaymentDateTimeUtc = uiPaymentHistory.PaymentDateTimeUtc;
        dbPaymentHistory.RecordedBy = uiPaymentHistory.RecordedBy;
        dbPaymentHistory.Comments = uiPaymentHistory.Comments;

        return dbPaymentHistory;
    }
}
