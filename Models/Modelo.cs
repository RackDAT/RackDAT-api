using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class Modelo : BaseModel
    {

        [PrimaryKey("id_modelo", false)]
        public int id { get; set; }
        [Required]
        public string modelo { get; set; }
        [Required]
        public int id_proveedor { get; set; }
    }
}
