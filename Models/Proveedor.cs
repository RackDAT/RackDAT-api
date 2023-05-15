using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("proveedor")]
    public class Proveedor : BaseModel
    {
        [PrimaryKey("id_proveedor", false)]
        public int id { get; set; }
        [Required, Column("proveedor")]
        public string proveedor { get; set; }
    }
}
