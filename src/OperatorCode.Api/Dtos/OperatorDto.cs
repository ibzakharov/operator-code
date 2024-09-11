using System.Text.Json.Serialization;

namespace OperatorCode.Api.Dtos;

public class OperatorDto : ModifyOperatorDto
{
    public int Code { get; set; }
}