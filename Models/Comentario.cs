namespace RackDAT_API.Models;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

[Table("Comentarios")]
public class Comentario : BaseModel
{
    [PrimaryKey("id_Comentario", false)]
    public int id { get; set; }
    [Required]
    public string comentario { get; set; }
}