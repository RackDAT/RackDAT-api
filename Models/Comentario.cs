using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class Comentario : BaseModel
    {
        [Required]
        public string comentario { get; set; }

    }
}
