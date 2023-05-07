namespace RackDAT_API.Contracts
{
    public class SolicitudLabResponse
    {
        public LabResponse lab { get; set; }
        public SolicitudResponse folio { get; set; }
        public DateTime inicio { get; set; }
        public DateTime final { get; set; }
        public int cantidad_personas { get; set; }
    }
}
