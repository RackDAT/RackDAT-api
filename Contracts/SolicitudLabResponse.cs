namespace RackDAT_API.Contracts
{
    public class SolicitudLabResponse
    {
#pragma warning disable CS8618 // Non-nullable property 'lab' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public LabResponse lab { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'lab' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'folio' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public SolicitudResponse folio { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'folio' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public DateTime inicio { get; set; }
        public DateTime final { get; set; }
        public int cantidad_personas { get; set; }
    }
}
