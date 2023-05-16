using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateSalonRequest
    {
        [Required]
#pragma warning disable CS8618 // Non-nullable property 'salon' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string salon { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'salon' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Required]
#pragma warning disable CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string descripcion { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    }
}
