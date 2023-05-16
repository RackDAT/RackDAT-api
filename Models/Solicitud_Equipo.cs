using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
#pragma warning disable CS0105 // The using directive for 'System.ComponentModel.DataAnnotations' appeared previously in this namespace
using System.ComponentModel.DataAnnotations;
#pragma warning restore CS0105 // The using directive for 'System.ComponentModel.DataAnnotations' appeared previously in this namespace
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RackDAT_API.Models
{
    [Table("solicitud_equipo")]
    public class Solicitud_Equipo : BaseModel
    {

        [PrimaryKey("id_solicitud", false)]
        public int folio { get; set; }

        [Reference(typeof(Solicitud))]
#pragma warning disable CS8618 // Non-nullable property 'solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Solicitud solicitud { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [PrimaryKey("id_equipo")]
        public int id_equipo { get; set; }

        [Reference(typeof(Equipo))]
#pragma warning disable CS8618 // Non-nullable property 'equipo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Equipo equipo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'equipo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public DateTime fecha_salida { get; set; }
        public DateTime fecha_vuelta { get; set; }

    }
}
