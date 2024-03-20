using Newtonsoft.Json;
using System;

namespace CCW.Common.Models;

public class Comment
{
    [JsonProperty("text")]
    public string Text { get; set; }
    [JsonProperty("commentDateTimeUtc")]
    public DateTimeOffset CommentDateTimeUtc { get; set; }
    [JsonProperty("commentMadeBy")]
    public string CommentMadeBy { get; set; }
}
