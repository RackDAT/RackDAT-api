namespace RackDAT_API.Contracts
{
    public class ReactivoEstanteriaResponse
    {
        public EstanteriaResponse estanteria { get; set; }
        public ReactivoResponse reactivo { get; set; }
        public float cantidad { get; set; }

    }
}
