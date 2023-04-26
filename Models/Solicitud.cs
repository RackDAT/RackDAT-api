using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;
namespace RackDAT_API.Models
{
    public class Solicitud : BaseModel
    {
        [PrimaryKey("id_solicitud", false)]
        public int id { get; set; }
        public TimeOnly fecha_actualizacion { get; set; }
        [Required]
        public int id_usuaio { get; set; }

        [Required]
        public int id_estatus_solicitud { get; set; }

        [Required]
        public int id_comentario { get; set; }

        [Required]
        public int id_tipo_solicitud { get; set; }
    }
}
