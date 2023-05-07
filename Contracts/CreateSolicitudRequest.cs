using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateSolicitudRequest
    {
        [Required]
        public int tipo_solicitud { get; set; }
        [Required]
        public int usuario { get; set; }
        public string comentario { get; set; }

    }
}
