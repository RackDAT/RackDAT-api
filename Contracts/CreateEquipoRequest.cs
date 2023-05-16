using Microsoft.VisualBasic;
using RackDAT_API.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RackDAT_API.Contracts;

public class CreateEquipoRequest
{
    [Required]
#pragma warning disable CS8618 // Non-nullable property 'num_serie' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string num_serie { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'num_serie' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    [Required]
#pragma warning disable CS8618 // Non-nullable property 'tag' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string tag { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'tag' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    [Required]
    public int id_modelo { get; set; }
    [Required]
    public DateOnly fecha_compra { get; set; }
    [Required]
#pragma warning disable CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string descripcion { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    [Required]
#pragma warning disable CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string imagen { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    [AllowNull]
    public string comentario { get; set; }
    [Required]
    public int estanteria { get; set; }

}
