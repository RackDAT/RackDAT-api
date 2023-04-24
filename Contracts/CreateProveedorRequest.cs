using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts;

public class CreateProveedorRequest
{
    [Required]
    public string nombre { get; set; }

}
