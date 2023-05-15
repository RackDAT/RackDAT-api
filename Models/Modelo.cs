using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("modelo")]
    public class Modelo : BaseModel
    {

        [PrimaryKey("id_modelo", false)]
        public int id { get; set; }
        [Required, Column("modelo")]
        public string modelo { get; set; }
        [Required, Column("id_proveedor"), ]
        public int id_proveedor { get; set; }
        [Reference(typeof(Proveedor))]
        public  Proveedor proveedor { get; set; }
    }
}
