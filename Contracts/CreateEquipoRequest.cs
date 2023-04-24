using RackDAT_API.Models;
using System.ComponentModel.DataAnnotations;
namespace RackDAT_API.Contracts;

public class CreateEquipoRequest
{
    [Required]
    public string num_serie { get; set; }
    [Required]
    public string tag { get; set; }
    [Required]
    public int modelo { get; set; }
    public int comentario { get; set; }
    [Required]
    public DateOnly fecha_compra { get; set; }
    public string descripcion { get; set; }

}
