namespace OperatorCode.Web.Models;

public class Response
{
    public bool Success { get; set; }
    public Dictionary<string, string> Errors { get; set; }
}