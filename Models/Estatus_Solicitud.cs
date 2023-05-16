using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("estatus_solicitud")]
    public class Estatus_Solicitud : BaseModel
    {
        [PrimaryKey("id_estatus_solicitud", false)]
        public int id { get; set; }
        [Required, Column("estatus_solicitud")]
#pragma warning disable CS8618 // Non-nullable property 'estatus_solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string estatus_solicitud { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'estatus_solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
       
    }
}
