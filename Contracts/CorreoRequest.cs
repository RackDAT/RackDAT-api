using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CorreoRequest
    {
        [Required]
#pragma warning disable CS8618 // Non-nullable property 'correo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string correo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'correo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
