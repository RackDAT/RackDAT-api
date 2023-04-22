namespace RackDAT_API.Models;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

[Table("Carreras")]
public class Carreras : BaseModel
{
    [PrimaryKey("id_Carreras", false)]
    public int id { get; set; }
    [Column("carrera"), Required]
    public string nombre { get; set; }
}
