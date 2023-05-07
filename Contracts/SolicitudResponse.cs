namespace RackDAT_API.Contracts
{
    public class SolicitudResponse
    {
        public int id { get; set; }
        public DateTime fecha_actualizacion { get; set; }
        public DateTime fecha_pedido { get; set; }
        public TipoSolicitudResponse tipo_solicitud { get; set; }
        public UsuarioResponse usuario { get; set; }
        public EstatusResponse estatus { get; set; }
        public string comentario { get; set; }
        public string imagen_muestra { get; set; }

    }
}
