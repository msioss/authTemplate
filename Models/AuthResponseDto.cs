using System.Text.Json.Serialization;

namespace StaffTracking.Models;

public class AuthResponseDto
{
    [JsonPropertyName("is_authorized")]
    public string IsAuthorized { get; set; }
}