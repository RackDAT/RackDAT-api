using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts;

public class CreateComentarioRequest
{
    [Required]
    public string comentario { get; set; }

}
