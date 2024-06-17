using System.Text.Json.Serialization;

namespace WhatsappDemoAPIs.Models;

public record StatusTargetUser
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("status_id")]
    public long StatusId { get; set; }

    [JsonPropertyName("is_seen")]
    public bool IsSeen { get; set; }

}