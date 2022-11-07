using Newtonsoft.Json;

namespace CCW.Application.Entities;

public class History
{
    [JsonProperty("change")]
    public string Change { get; set; }
    [JsonProperty("changeDateTimeUtc")]
    public DateTime ChangeDateTimeUtc { get; set; }
    [JsonProperty("changeMadeBy")]
    public string ChangeMadeBy { get; set; }
}


public class HistoryResponse
{
   public History[] History { get; set; }
}

public class HistoryProps
{
    [JsonProperty("change")]
    public string Change { get; set; }
    [JsonProperty("changeDateTimeUtc")]
    public DateTime ChangeDateTimeUtc { get; set; }
    [JsonProperty("changeMadeBy")]
    public string ChangeMadeBy { get; set; }
}
