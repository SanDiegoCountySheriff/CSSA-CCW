using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class RequestPermitApplicationToPaymentHistoryMapper : IMapper<PermitApplicationRequestModel, PaymentHistory[]>
{
    public PaymentHistory[] Map(PermitApplicationRequestModel source)
    {
        if (source.PaymentHistory != null && source.PaymentHistory.Length > 0)
        {
            int count = source.PaymentHistory.Length;
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
        dbPaymentHistory.PaymentDateTimeUtc = uiPaymentHistory.PaymentDateTimeUtc;
        dbPaymentHistory.PaymentType = uiPaymentHistory.PaymentType;
        dbPaymentHistory.VendorInfo = uiPaymentHistory.VendorInfo;
        dbPaymentHistory.Amount = uiPaymentHistory.Amount;
        dbPaymentHistory.RecordedBy = uiPaymentHistory.RecordedBy;
        dbPaymentHistory.TransactionId = uiPaymentHistory.TransactionId;

        return dbPaymentHistory;
    }
}
