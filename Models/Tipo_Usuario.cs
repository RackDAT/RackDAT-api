using Postgrest.Attributes;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("tipo_usuario")]
    public class Tipo_Usuario :  BaseModel
    {
        [PrimaryKey("id_tipo_usuario", false)]
        public int id { get; set; }
        [Required]
        public string tipo_usuario { get; set; }
    }
}
