using System.Text.Json.Serialization;
using WhatsappDemoAPIs.Models;

namespace WhatsappDemoAPIs.DTOs;

public class ChatDto
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
}

public class ChatCreateDto
{
    // [JsonPropertyName("sender_id")]
    // public long SenderId { get; set; }

    [JsonPropertyName("reciever_id")]
    public long RecieverId { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;

    [JsonPropertyName("is_group_chat")]
    public bool IsGroupChat { get; set; }

    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }
}

public class ChatUpdateDto
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;

    [JsonPropertyName("is_group_chat")]
    public bool IsGroupChat { get; set; }

    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }
}