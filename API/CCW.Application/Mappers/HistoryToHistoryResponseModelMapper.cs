using CCW.Application.Entities;
using CCW.Application.Models;

namespace CCW.Application.Mappers;

public class HistoryToHistoryResponseModelMapper : IMapper<History, HistoryResponseModel>
{
    public HistoryResponseModel Map(History source)
    {
        return new HistoryResponseModel
        {
            Change = source.Change,
            ChangeDateTimeUtc = source.ChangeDateTimeUtc,
            ChangeMadeBy = source.ChangeMadeBy,
        };
    }
}