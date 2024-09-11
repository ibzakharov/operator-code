namespace OperatorCode.Api.Models;

public class ErrorCode
{
    public int Status { get; set; }
    public string Title { get; set; }
    public string Detail { get; set; }
    public List<Dictionary<string, string>> Errors { get; set; }
}