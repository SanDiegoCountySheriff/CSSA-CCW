using CCW.Common.Enums;
using Newtonsoft.Json;

namespace CCW.Common.ResponseModels;

public class HistoricalApplicationSummaryResponseModel
{
    public string Id { get; set; }
    public DateTimeOffset HistoricalDate { get; set; }
    public ApplicationType ApplicationType { get; set; }
}
