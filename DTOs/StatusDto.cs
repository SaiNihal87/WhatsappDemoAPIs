using System.Text.Json.Serialization;
using WhatsappDemoAPIs.Models;

namespace WhatsappDemoAPIs.DTOs;

public class StatusDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    // [JsonPropertyName("posted_by_user_id")]
    // public long PostedByUserId { get; set; }

    [JsonPropertyName("media_url")]
    public string MediaUrl { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}

public class StatusCreateDto
{
    // [JsonPropertyName("posted_by_user_id")]
    // public long PostedByUserId { get; set; }

    [JsonPropertyName("media_url")]
    public string MediaUrl { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;
}

public record StatusTargetUsersDto
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("status_id")]
    public long StatusId { get; set; }

    [JsonPropertyName("is_seen")]
    public bool IsSeen { get; set; }
}

public class StatusTargetUsersCreateDto
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("status_id")]
    public long StatusId { get; set; }

    [JsonPropertyName("is_seen")]
    public bool IsSeen { get; set; }
}

public class StatusTargetUsersUpdateDto
{
    [JsonPropertyName("is_seen")]
    public bool IsSeen { get; set; }
}

