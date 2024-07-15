using Newtonsoft.Json;

namespace CCW.Common.ResponseModels;

public class HistoricalApplicationSummaryResponseModel
{
    public string Id { get; set; }
    public DateTimeOffset HistoricalDate { get; set; }
}
