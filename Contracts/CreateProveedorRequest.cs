using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts;

public class CreateProveedorRequest
{
    [Required]
    public string proveedor { get; set; }

}
