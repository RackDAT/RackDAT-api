namespace RackDAT_API.Contracts
{
    public class SolicitudResponse
    {
        public int id { get; set; }
        public DateTime fecha_actualizacion { get; set; }
        public int tipo_Solicitud { get; set; }
        public int usuario { get; set; }
        public int estatus { get; set; }
        public string comentarios { get; set; }

    }
}
