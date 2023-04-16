using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts;
public class CreateCarreraRequest
{
    [Required]
    public string carrera { get; set; }

}
