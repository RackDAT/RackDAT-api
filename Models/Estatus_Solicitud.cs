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
        public string estatus_solicitud { get; set; }
       
    }
}
