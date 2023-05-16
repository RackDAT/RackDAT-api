using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateUsuarioRequest
    {
        [Required]
#pragma warning disable CS8618 // Non-nullable property 'nombre' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string nombre { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'nombre' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required]
#pragma warning disable CS8618 // Non-nullable property 'apellido_pat' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string apellido_pat { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'apellido_pat' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'apellido_mat' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string apellido_mat { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'apellido_mat' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required]
#pragma warning disable CS8618 // Non-nullable property 'correo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string correo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'correo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required]
#pragma warning disable CS8618 // Non-nullable property 'clave' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string clave { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'clave' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required]
        public int tipo_usuario { get; set; }
        [Required]
        public int carrera { get; set; }
        [Required]
#pragma warning disable CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string imagen { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    }
}
