using Microsoft.VisualBasic;
using RackDAT_API.Models;
using System.Diagnostics.CodeAnalysis;

namespace RackDAT_API.Contracts;

public class EquipoResponse
{
    public int id { get; set; }
    public string num_serie { get; set; }    
    public string tag { get; set; }    
    public ModeloResponse modelo { get; set; }
    [MaybeNull]
    public string comentario { get; set; }    
    public DateOnly fecha_compra { get; set; }
    public string descripcion { get; set; }
    public string imagen { get; set; }
    public int estanteria { get; set; }

}