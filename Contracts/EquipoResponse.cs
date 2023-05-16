using Microsoft.VisualBasic;
using RackDAT_API.Models;
using System.Diagnostics.CodeAnalysis;

namespace RackDAT_API.Contracts;

public class EquipoResponse
{
    public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'num_serie' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string num_serie { get; set; }    
#pragma warning restore CS8618 // Non-nullable property 'num_serie' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'tag' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string tag { get; set; }    
#pragma warning restore CS8618 // Non-nullable property 'tag' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'modelo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public ModeloResponse modelo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'modelo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    [MaybeNull]
    public string comentario { get; set; }    
    public DateOnly fecha_compra { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string descripcion { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string imagen { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'estanteria' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public EstanteriaResponse estanteria { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'estanteria' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

}