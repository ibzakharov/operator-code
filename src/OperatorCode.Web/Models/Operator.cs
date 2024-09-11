using System.ComponentModel.DataAnnotations;

namespace OperatorCode.Web.Models;

public class Operator
{
    public int Code { get; set; }
    [Required]
    public string Name { get; set; }
}