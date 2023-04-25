namespace RackDAT_API.Models;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

[Table("Carreras")]
public class Carrera : BaseModel
{
    [PrimaryKey("id_carrera", false)]
    public int id_carrera { get; set; }
    [Required]
    public string carrera { get; set; }
}
