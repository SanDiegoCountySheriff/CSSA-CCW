using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class RequestPermitApplicationToHistoryMapper : IMapper<PermitApplicationRequestModel, History[]>
{
    public History[] Map(PermitApplicationRequestModel source)
    {
        if (source.History != null && source.History.Length > 0)
        {
            int count = source.History.Length;
            var newItem = new History[count];
            for (int i = 0; i < count; i++)
            {
                newItem[i] = MapAlias(source.History[i], new History());
            }

            return newItem;
        }

        return Array.Empty<History>();
    }

    private static History MapAlias(History uiHistory, History dbHistory)
    {
        dbHistory.Change = uiHistory.Change;
        dbHistory.ChangeDateTimeUtc = uiHistory.ChangeDateTimeUtc;
        dbHistory.ChangeMadeBy = uiHistory.ChangeMadeBy;

        return dbHistory;
    }
}
