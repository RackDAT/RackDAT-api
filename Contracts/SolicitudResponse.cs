namespace RackDAT_API.Contracts
{
    public class SolicitudResponse
    {
        public int id { get; set; }
        public DateTime fecha_pedido { get; set; }
        public DateTime fecha_salida { get; set; }
        public DateTime fecha_vuelta { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'tipo_solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public TipoSolicitudResponse tipo_solicitud { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'tipo_solicitud' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'usuario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public UsuarioResponse usuario { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'usuario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'estatus' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public EstatusResponse estatus { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'estatus' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'comentario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string comentario { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'comentario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'imagen_muestra' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string imagen_muestra { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'imagen_muestra' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public int cantidad_equipos { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'lab' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public LabResponse lab { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'lab' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'equipos' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public List<EquipoResponse> equipos { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'equipos' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public int cantidad_personas { get; set; }

    }
}
