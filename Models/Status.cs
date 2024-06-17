using System.Text.Json.Serialization;

namespace WhatsappDemoAPIs.Models;

public record Status
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("posted_by_user_id")]
    public long PostedByUserId { get; set; }

    [JsonPropertyName("media_url")]
    public string MediaUrl { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("status_target_users")]
    public string? StatusTargetUsers { get; set; }

    // [JsonPropertyName("is_seen")]
    // public bool IsSeen { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}