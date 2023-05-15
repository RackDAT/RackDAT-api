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
        [Required, Column("nombre")]
        public string nombre { get; set; }

        [Required, Column("apellido_pat")]
        public string apellido_pat { get; set; }
        [Column("apellido_mat")]
        public string? apellido_mat { get; set; }

        [Required, Column("correo")]
        public string correo { get; set; }

        [Required, Column("clave")]
        public string clave { get; set; }

        [Required, Column("verificado")]
        public bool verificado { get; set; }

        [Required, Column("id_tipo_usuario")]
        public int id_tipo_usuario { get; set; }
        [Reference(typeof(Tipo_Usuario))]
        public Tipo_Usuario tipo_usuario { get; set; }
        [Column("id_carrera")]
        public int id_carrera { get; set; }

        [Reference(typeof(Carrera))]
        public Carrera carrera { get; set; }
        [Required, Column("imagen")]
        public string imagen { get; set; }
    }
}
