namespace RackDAT_API.Contracts
{
    public class ReactivoEstanteriaResponse
    {
#pragma warning disable CS8618 // Non-nullable property 'estanteria' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public EstanteriaResponse estanteria { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'estanteria' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'reactivo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public ReactivoResponse reactivo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'reactivo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public float cantidad { get; set; }

    }
}
