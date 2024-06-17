using System.Text.Json.Serialization;

namespace WhatsappDemoAPIs.Models;

public record Groups
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("created_by_user_id")]
    public long CreatedByUserId { get; set; }

    [JsonPropertyName("is_public")]

    public bool IsPublic {get; set;}

    [JsonPropertyName("users")]
    public string? Users { get; set; }

    [JsonPropertyName("chat")]
    public string? Chat { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
}