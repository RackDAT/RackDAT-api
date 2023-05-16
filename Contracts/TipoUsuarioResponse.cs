namespace RackDAT_API.Contracts
{
    public class TipoUsuarioResponse
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'tipo_usuario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string tipo_usuario { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'tipo_usuario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }

}
