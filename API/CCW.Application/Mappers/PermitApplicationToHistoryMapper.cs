using CCW.Application.Entities;

namespace CCW.Application.Mappers;

public class PermitApplicationToHistoryMapper : IMapper<PermitApplication, History[]>
{
    public History[] Map(PermitApplication source)
    {
        if (source.History != null)
        {
            int count = source.History.Length;
            var newItem = new History[count];
            for (int i = 0; i < count; i++)
            {
                newItem[i] = MapAlias(source.History[i], new History());
            }

            return newItem;
        }

        return new History[0];
    }

    private static History MapAlias(History uiHistory, History dbHistory)
    {
        dbHistory.Change = uiHistory.Change;
        dbHistory.ChangeDateTimeUtc = uiHistory.ChangeDateTimeUtc;
        dbHistory.ChangeMadeBy = uiHistory.ChangeMadeBy;

        return dbHistory;
    }
}