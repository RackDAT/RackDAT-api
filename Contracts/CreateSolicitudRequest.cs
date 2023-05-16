using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateSolicitudRequest
    {
        [Required]
        public int tipo_solicitud { get; set; }
        [Required]
        public int usuario { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'comentario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string comentario { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'comentario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    }
}
