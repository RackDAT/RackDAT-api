using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CarreraResponse
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'carrera' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string carrera { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'carrera' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'siglas' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string siglas { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'siglas' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
