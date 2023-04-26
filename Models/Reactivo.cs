using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class Reactivo : BaseModel
    {
        [PrimaryKey("id_reactivo", false)]
        public string id { get; set; }
        public string reactivo { get; set; }
        public int id_unidad_medida { get; set; }
    }
}
