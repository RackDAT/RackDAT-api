using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class Modelo : BaseModel
    {

        [PrimaryKey]
        public int id_modelo { get; set; }
        [Required]
        public string modelo { get; set; }
        [Required]
        public int id_proveedor { get; set; }
    }
}
