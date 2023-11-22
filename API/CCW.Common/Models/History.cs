using Newtonsoft.Json;
using System;

namespace CCW.Common.Models;

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
