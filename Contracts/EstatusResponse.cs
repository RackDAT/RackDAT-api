namespace RackDAT_API.Contracts
{
    public class EstatusResponse
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'estatus' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string estatus { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'estatus' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    }

}
