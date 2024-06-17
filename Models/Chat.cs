using System.Text.Json.Serialization;

namespace WhatsappDemoAPIs.Models;

public record Chat
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("sender_id")]
    public long SenderId { get; set; }

    [JsonPropertyName("reciever_id")]
    public long RecieverId { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;

    [JsonPropertyName("is_group_chat")]
    public bool IsGroupChat { get; set; }

    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }
}