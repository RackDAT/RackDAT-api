using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("tipo_usuario")]
    public class Tipo_Solicitud : BaseModel
    {
        [PrimaryKey("id_tipo_solicitud", false)]
        public int id { get; set; }
        [Required, Column("tipo_solicitud")]
        public string tipo_solicitud { get; set; }
    }
}
