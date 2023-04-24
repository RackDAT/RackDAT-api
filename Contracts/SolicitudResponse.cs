namespace RackDAT_API.Contracts
{
    public class SolicitudResponse
    {
        public int id { get; set; }
        public DateTime fecha_actualizacion { get; set; }
        public TipoSolicitudResponse tipo_solicitud { get; set; }
        public UsuarioResponse usuario { get; set; }
        public EstatusResponse estatus { get; set; }
        public ComentariosResponse comentario { get; set; }

    }
}
