using System.Text.Json.Serialization;

namespace WhatsappDemoAPIs.Models;

public record User
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("about")]
    public string About { get; set; } = null!;

    [JsonPropertyName("phone")]
    public long Phone { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    [JsonPropertyName("profile_url")]
    public string? ProfileUrl { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("calls")]
    public string? Calls { get; set; }

    [JsonPropertyName("chat")]
    public string? Chat { get; set; }

    [JsonPropertyName("groups")]
    public string? Groups { get; set; }
   
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

}