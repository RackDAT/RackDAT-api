using Microsoft.VisualBasic;
using RackDAT_API.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RackDAT_API.Contracts;

public class CreateEquipoRequest
{
    [Required]
    public string num_serie { get; set; }
    [Required]
    public string tag { get; set; }
    [Required]
    public int modelo { get; set; }    
    [Required]
    public DateOnly fecha_compra { get; set; }
    [Required]
    public string descripcion { get; set; }
    [Required]
    public string imagen { get; set; }
    [AllowNull]
    public string comentario { get; set; }

}
