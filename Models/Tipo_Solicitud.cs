using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("tipo_solicitud")]
    public class Tipo_Solicitud : BaseModel
    {
        [PrimaryKey("id_tipo_solicitud", false)]
        public int id { get; set; }
        [Required, Column("tipo_solicitud")]
#pragma warning disable CS8618 // Non-nullable property 'tipo_solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string tipo_solicitud { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'tipo_solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
