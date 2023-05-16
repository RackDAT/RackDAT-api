using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RackDAT_API.Models
{
    [Table("solicitud_equipo")]
    public class Solicitud_Equipo : BaseModel
    {

        [Column("id_solicitud")]
        public int folio { get; set; }

        [Reference(typeof(Solicitud))]
        public Solicitud solicitud { get; set; }

        [Column("id_equipo")]
        public int id_equipo { get; set; }

        [Reference(typeof(Equipo))]
        public Equipo equipo { get; set; }
        public DateTime fecha_salida { get; set; }
        public DateTime fecha_vuelta { get; set; }

    }
}
