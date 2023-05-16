using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;

namespace RackDAT_API.Models
{
    [Table("solicitud_laboratorio")]
    public class Solicitud_Lab : BaseModel
    {
        [Column("id_solicitud")]
        public int id_solicitud { get; set; }
        [Reference(typeof(Solicitud))]
#pragma warning disable CS8618 // Non-nullable property 'solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Solicitud solicitud { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Column("id_laboratorio")]
        public int id_laboratorio { get; set; }
        [Reference(typeof(Laboratorio))]
#pragma warning disable CS8618 // Non-nullable property 'laboratorio' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Laboratorio laboratorio { set; get; }
#pragma warning restore CS8618 // Non-nullable property 'laboratorio' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Column("fecha_inicio")]
        public DateTime fecha_salida { get; set; }
        [Column("fecha_final")]
        public DateTime fecha_vuelta { get; set; }
        [Column("cantidad_personas")]
        public int cantidad_personas { get; set; }

    }
}
