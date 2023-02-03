using CCW.Application.Entities;

namespace CCW.Application.Mappers;

    public class PermitApplicationToPaymentHistoryMapper : IMapper<PermitApplication, PaymentHistory[]>
    {
        public PaymentHistory[] Map(PermitApplication source)
        {
            if (source?.PaymentHistory != null)
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
            dbPaymentHistory.Amount = uiPaymentHistory.Amount;
            dbPaymentHistory.PaymentDateTimeUtc = uiPaymentHistory.PaymentDateTimeUtc;
            dbPaymentHistory.RecordedBy = uiPaymentHistory.RecordedBy;
            dbPaymentHistory.Comments = uiPaymentHistory.Comments;

            return dbPaymentHistory;
        }
    }