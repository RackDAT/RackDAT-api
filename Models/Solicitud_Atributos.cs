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
#pragma warning disable CS8618 // Non-nullable property 'usuario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Usuario usuario { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'usuario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Required, Column("id_estatus_solicitud")]
        public int id_estatus_solicitud { get; set; }
        [Reference(typeof(Estatus_Solicitud))]
#pragma warning disable CS8618 // Non-nullable property 'estatus_solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Estatus_Solicitud estatus_solicitud { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'estatus_solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Required, Column("comentario")]
#pragma warning disable CS8618 // Non-nullable property 'comentario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string comentario { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'comentario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Required, Column("id_tipo_solicitud")]
        public int id_tipo_solicitud { get; set; }
        [Reference(typeof(Tipo_Solicitud))]
#pragma warning disable CS8618 // Non-nullable property 'tipo_solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Tipo_Solicitud tipo_solicitud { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'tipo_solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

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
        [Column("carrera")]
        public int carrera { get; set; }
    }
}
