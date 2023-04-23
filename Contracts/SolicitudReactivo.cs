namespace RackDAT_API.Contracts
{
    public class SolicitudReactivo
    {
        public ReactivoResponse reactivo { get; set; }
        public SolicitudResponse folio { get; set; }
        public float cantidad { get; set; }
        public DateOnly fecha_salida { get; set; }

    }
}
