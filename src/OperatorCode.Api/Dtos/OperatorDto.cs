using System.Text.Json.Serialization;

namespace OperatorCode.Api.Dtos;

public class OperatorDto
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
}