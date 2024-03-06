using Newtonsoft.Json;

namespace CCW.Common.Models;

public class UserTransaction
{
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("userId")]
    public string UserId { get; set; }
    [JsonProperty("transactionIdentifier")]
    public Guid TransactionIdentifier { get; set; }
    [JsonProperty("transactionIdentifierUsed")]
    public bool TransactionIdentifierUsed { get; set; }
}