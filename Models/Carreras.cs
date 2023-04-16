namespace RackDAT_API.Models;
using Postgrest.Attributes;
using Postgrest.Models;
[Table("Carreras")]
public class Carreras
{
    [PrimaryKey("id", false)]
    public int id { get; set; }
    public string carrera { get; set; }
}
