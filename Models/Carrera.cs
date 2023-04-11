using Postgrest.Attributes;
using Postgrest.Models;

namespace RackDAT_API.Models;

[Table("Carreras")]
public class Carrera : BaseModel
{
    [PrimaryKey("id_Carreras")]
    public int ID { get; set; }
    [Column("carrera")]
    public string carrera { get; set;}
}

