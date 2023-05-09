using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("salon")]
    public class Salon : BaseModel
    {
        [PrimaryKey("id_salon", false)]
        public int id { get; set; }
        [Required]
        public string salon { get; set; }
        [Required]
        public string descripcion { get; set; }
    }
}
