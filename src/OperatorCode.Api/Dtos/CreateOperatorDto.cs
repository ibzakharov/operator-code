using System.Text.Json.Serialization;

namespace OperatorCode.Api.Dtos;

public class CreateOperatorDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}