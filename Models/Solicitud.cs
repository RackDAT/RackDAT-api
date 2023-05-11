using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RackDAT_API.Models
{
    [Table("solicitud")]
    public class Solicitud : BaseModel
    {
        [PrimaryKey("id_solicitud", false)]
        public int folio { get; set; }
        [Column("fecha_pedido")]
        public DateTime fecha_pedido { get; set; }

        [Required]
        public int id_usuario { get; set; }

        [Required, Column("id_estatus_solicitud")]
        public int id_estatus_solicitud { get; set; }

        [Required]
        public string comentario { get; set; }

        [Required]
        public int id_tipo_solicitud { get; set; }
        [Column("aprobacion_coordinador")]
        public int aprobacion_coordinador { get; set; }
        [Column("aprobacion_tecnico")]
        public int aprobacion_tecnico { get; set; }
    }
}
