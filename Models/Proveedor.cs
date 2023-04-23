using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class Proveedor : BaseModel
    {
        [PrimaryKey]
        public int id_proveedor { get; set; }
        [Required]
        public string proveedor { get; set; }
    }
}
