using System.Text.Json.Serialization;

namespace WhatsappDemoAPIs.Models;

public record GroupMember
{ 
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }

    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    [JsonPropertyName("is_admin")]
    public bool IsAdmin { get; set; }

}