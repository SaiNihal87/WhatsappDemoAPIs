using System;
using System.Text.Json.Serialization;
using WhatsappDemoAPIs.Models;

namespace WhatsappDemoAPIs.DTOs
{
    public class GroupsDto
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

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset? UpdatedAt { get; set; }
    }

    public class GroupsCreateDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        // [JsonPropertyName("created_by_user_id")]
        // public long CreatedByUserId { get; set; }

        [JsonPropertyName("is_public")]
        public bool IsPublic {get; set;}
    }

    public class GroupsUpdateDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("is_public")]
        public bool IsPublic {get; set;}
    }

    public class GroupMembersDto
    {
        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("group_id")]
        public long GroupId { get; set; }

        [JsonPropertyName("is_admin")]
        public bool IsAdmin { get; set; }
    }


    public class GroupMembersCreateDto
    {
        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("group_id")]
        public long GroupId { get; set; }

        [JsonPropertyName("is_admin")]
        public bool IsAdmin { get; set; }
    }

    public class GroupMembersUpdateDto
    {
        [JsonPropertyName("is_admin")]
        public bool IsAdmin { get; set; }
    }
}
