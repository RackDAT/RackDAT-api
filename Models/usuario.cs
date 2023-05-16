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
#pragma warning disable CS8618 // Non-nullable property 'nombre' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string nombre { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'nombre' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Required, Column("apellido_pat")]
#pragma warning disable CS8618 // Non-nullable property 'apellido_pat' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string apellido_pat { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'apellido_pat' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Column("apellido_mat")]
        public string? apellido_mat { get; set; }

        [Required, Column("correo")]
#pragma warning disable CS8618 // Non-nullable property 'correo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string correo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'correo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Required, Column("clave")]
#pragma warning disable CS8618 // Non-nullable property 'clave' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string clave { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'clave' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Required, Column("verificado")]
        public bool verificado { get; set; }

        [Required, Column("id_tipo_usuario")]
        public int id_tipo_usuario { get; set; }
        [Reference(typeof(Tipo_Usuario))]
#pragma warning disable CS8618 // Non-nullable property 'tipo_usuario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Tipo_Usuario tipo_usuario { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'tipo_usuario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Column("id_carrera")]
        public int id_carrera { get; set; }

        [Reference(typeof(Carrera))]
#pragma warning disable CS8618 // Non-nullable property 'carrera' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Carrera carrera { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'carrera' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required, Column("imagen")]
#pragma warning disable CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string imagen { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
