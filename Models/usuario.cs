using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("usuario")]
    public class Usuario : BaseModel
    {
        [PrimaryKey("id_usuario", false)]
        public int id { get; set; }
        [Required]
        public string nombre { get; set; }

        [Required]
        public string apellido_pat { get; set; }

        public string apellido_mat { get; set; }

        [Required, Column("correo")]
        public string correo { get; set; }

        [Required]
        public string clave { get; set; }

        [Required, Column("verificado")]
        public bool verificado { get; set; }

        [Required]
        public int id_tipo_usuario { get; set; }
        public int id_carrera { get; set; }

        [Reference(typeof(Carrera))]
        public Carrera carrera { get; set; }
        [Required]
        public string imagen { get; set; }
    }
}
