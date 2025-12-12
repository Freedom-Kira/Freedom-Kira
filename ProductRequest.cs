using System.Text.Json.Serialization;

namespace api_snake_case.Models;

public record ProductRequest
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("agency_code")]
    public required string AgencyCode { get; set; }

    [JsonPropertyName("audio_enable")]
    public bool AudioEnable { get; set; }

}