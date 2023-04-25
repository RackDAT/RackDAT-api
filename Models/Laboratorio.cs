using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class Laboratorio : BaseModel
    {

        [PrimaryKey]
        public int id_laboratorio { get; set; }
        [Required]
        public string laboratorio { get; set; }
        [Required]
        public int id_salon { get; set; }
    }
}
