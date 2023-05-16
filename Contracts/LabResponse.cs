using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class LabResponse
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'lab' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string lab { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'lab' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'salon' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public SalonResponse salon { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'salon' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string imagen { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string descripcion { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
