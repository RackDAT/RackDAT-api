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
    public ICollection<Comentario> comentarios { get; set; }

}