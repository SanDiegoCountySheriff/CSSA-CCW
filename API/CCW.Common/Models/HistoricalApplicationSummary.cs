using Newtonsoft.Json;

namespace CCW.Common.Models;

public class HistoricalApplicationSummary
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public DateTimeOffset HistoricalDate { get; set; }
}
