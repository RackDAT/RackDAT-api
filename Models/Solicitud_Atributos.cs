using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("solicitudes_atributos")]
    public class Solicitud_Atributos : BaseModel
    {
        [PrimaryKey("id_solicitud", false)]
        public int folio { get; set; }
        [Column("fecha_pedido")]
        public DateTime fecha_pedido { get; set; }

        [Required, Column("id_usuario")]
        public int id_usuario { get; set; }
        [Reference(typeof(Usuario))]
        public Usuario usuario { get; set; }

        [Required, Column("id_estatus_solicitud")]
        public int id_estatus_solicitud { get; set; }
        [Reference(typeof(Estatus_Solicitud))]
        public Estatus_Solicitud estatus_solicitud { get; set; }

        [Required, Column("comentario")]
        public string comentario { get; set; }

        [Required, Column("id_tipo_solicitud")]
        public int id_tipo_solicitud { get; set; }
        [Reference(typeof(Tipo_Solicitud))]
        public Tipo_Solicitud tipo_solicitud { get; set; }

        [Column("aprobacion_coordinador")]
        public bool? aprobacion_coordinador { get; set; }

        [Column("aprobacion_tecnico")]
        public bool? aprobacion_tecnico { get; set; }
        [Column("cantidad_equipos")]
        public int? cantidad_equipos { get; set; }
        [Column("imagen")]
        public string? imagen { get; set; }
        [Column("laboratorio_obtenido")]
        public string? laboratorio_obtenido { get; set; }
    }
}
