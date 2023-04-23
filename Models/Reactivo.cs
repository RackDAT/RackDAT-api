using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class Reactivo : BaseModel
    {
        [Required]
        public string id_reactivo { get; set; }
        public string reactivo { get; set; }
        public int id_unidad_medida { get; set; }
    }
}
