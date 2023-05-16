using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateReactivoRequest
    {
        [Required]
#pragma warning disable CS8618 // Non-nullable property 'nombre' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string nombre { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'nombre' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required]
        public int medida { get; set; }

    }
}
