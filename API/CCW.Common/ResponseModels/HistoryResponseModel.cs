using Newtonsoft.Json;

namespace CCW.Application.ResponseModels;

public class HistoryResponseModel
{
    [JsonProperty("change")]
    public string Change { get; set; }
    [JsonProperty("changeDateTimeUtc")]
    public DateTimeOffset ChangeDateTimeUtc { get; set; }
    [JsonProperty("changeMadeBy")]
    public string ChangeMadeBy { get; set; }
}