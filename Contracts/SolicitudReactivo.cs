namespace RackDAT_API.Contracts
{
    public class SolicitudReactivo
    {
#pragma warning disable CS8618 // Non-nullable property 'reactivo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public ReactivoResponse reactivo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'reactivo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'folio' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public SolicitudResponse folio { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'folio' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public float cantidad { get; set; }
        public DateOnly fecha_salida { get; set; }

    }
}
