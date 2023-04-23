using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class Comentario : BaseModel
    {
        [PrimaryKey("id_Carreras", false)]
        public int id_comentario { get; set; }

        [Required]
        public string comentario { get; set; }

    }
}
