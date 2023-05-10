using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("solicitud_laboratorio")]
    public class Solicitud_Lab : BaseModel
    {
        [Column("id_solicitud")]
        public int folio { get; set; }
        [Column("id_laboratorio")]
        public int laboratorio { get; set; }
        [Column("fecha_inicio")]
        public DateTime fecha_salida { get; set; }
        [Column("fecha_final")]
        public DateTime fecha_vuelta { get; set; }
        public int cantidad_personas { get; set; }

    }
}
