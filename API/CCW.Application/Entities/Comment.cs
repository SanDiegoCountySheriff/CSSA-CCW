using Newtonsoft.Json;

namespace CCW.Application.Entities;

public class Comment
{
    [JsonProperty("text")]
    public string Text { get; set; }
    [JsonProperty("commentDateTimeUtc")]
    public DateTime CommentDateTimeUtc { get; set; }
    [JsonProperty("commentMadeBy")]
    public string CommentMadeBy { get; set; }
}
