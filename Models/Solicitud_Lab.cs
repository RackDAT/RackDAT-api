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
        public Solicitud solicitud { get; set; }

        [Column("id_laboratorio")]
        public int id_laboratorio { get; set; }
        [Reference(typeof(Laboratorio))]
        public Laboratorio laboratorio { set; get; }
        [Column("fecha_inicio")]
        public DateTime fecha_salida { get; set; }
        [Column("fecha_final")]
        public DateTime fecha_vuelta { get; set; }
        [Column("cantidad_personas")]
        public int cantidad_personas { get; set; }

    }
}
