namespace RackDAT_API.Contracts
{
    public class EquipoEstanteriaResponse
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'equipo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public EquipoResponse equipo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'equipo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'estanteria' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public EstanteriaResponse estanteria { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'estanteria' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    }
}
