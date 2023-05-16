namespace RackDAT_API.Contracts
{
    public class UsuarioResponse
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'nombre' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string nombre { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'nombre' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'apellido_pat' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string apellido_pat { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'apellido_pat' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'apellido_mat' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string apellido_mat { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'apellido_mat' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'correo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string correo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'correo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'clave' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string clave { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'clave' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public bool verificado { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'tipo_usuario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public TipoUsuarioResponse tipo_usuario { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'tipo_usuario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'carrera' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public CarreraResponse carrera { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'carrera' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string imagen { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    }

}
