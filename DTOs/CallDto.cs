using System.Text.Json.Serialization;

namespace WhatsappDemoAPIs.DTOs;

public class CallDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("caller_id")]
    public long CallerId { get; set; }

    [JsonPropertyName("reciever_id")]
    public long RecieverId { get; set; }

    [JsonPropertyName("caller_phone")]
    public long CallerPhone { get; set; }

    [JsonPropertyName("reciever_phone")]
    public long RecieverPhone { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}

public class CallCreateDto
{
    // [JsonPropertyName("caller_id")]
    // public long CallerId { get; set; }

    [JsonPropertyName("reciever_id")]
    public long RecieverId { get; set; }
}
