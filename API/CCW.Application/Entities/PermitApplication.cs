using Newtonsoft.Json;

namespace CCW.Application.Entities;

public class PermitApplication
{
    public Application Application { get; set; }

    [JsonProperty("id")]
    public Guid Id { get; set; }
}