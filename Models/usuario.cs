using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class usuario : BaseModel
    {
        [PrimaryKey]
        public int id_usuario { get; set; }
        [Required]
        public string usuario { get; set; }

        [Required]
        public string apellido_pat { get; set; }

        [Required]
        public string apellido_mat { get; set; }

        [Required]
        public string correo { get; set; }

        [Required]
        public string clave { get; set; }

        [Required]
        public bool verificado { get; set; }

        [Required]
        public int id_tipo_usuario { get; set; }

        public int id_carrera { get; set; }
    }
}
