namespace RackDAT_API.Contracts
{
    public class ReactivoResponse
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'nombre' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string nombre { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'nombre' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'medida' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public UMResponse medida { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'medida' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
