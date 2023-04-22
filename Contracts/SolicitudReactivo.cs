namespace RackDAT_API.Contracts
{
    public class SolicitudReactivo
    {
        public int reactivo { get; set; }
        public int folio { get; set; }
        public float cantidad { get; set; }
        public DateOnly fecha_salida { get; set; }

    }
}
