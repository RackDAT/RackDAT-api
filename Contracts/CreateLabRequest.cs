using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateLabRequest
    {
        [Required]
#pragma warning disable CS8618 // Non-nullable property 'lab' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string lab { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'lab' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required]
        public int salon { get; set; }
        [Required]
#pragma warning disable CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string imagen { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string descripcion { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    }
}
