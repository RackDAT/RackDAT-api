using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts;

public class CreateDocumentoRequest
{
    [Required]
    public string link { get; set; }
    [Required]
    public int tipoDocs { get; set; }

}
