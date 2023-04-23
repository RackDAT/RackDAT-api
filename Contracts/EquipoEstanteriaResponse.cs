namespace RackDAT_API.Contracts
{
    public class EquipoEstanteriaResponse
    {
        public int id { get; set; }
        public EquipoResponse equipo { get; set; }
        public EstanteriaResponse estanteria { get; set; }

    }
}
