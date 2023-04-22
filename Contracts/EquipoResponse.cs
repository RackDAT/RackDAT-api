using RackDAT_API.Models;
namespace RackDAT_API.Contracts;

public class EquipoResponse
{
    public int id { get; set; }
    public string num_serie { get; set; }    
    public string tag { get; set; }    
    public int modelo { get; set; }
    public string comentarios { get; set; }    
    public DateOnly fecha_compra { get; set; }
    public string descripcion { get; set; }

}