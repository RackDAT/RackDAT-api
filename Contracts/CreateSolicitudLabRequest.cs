using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateSolicitudLabRequest
    {

        [Required]
        public int lab { get; set; }
        [Required]
        public int folio { get; set; }
        [Required]
        public DateTime inicio { get; set; }
        [Required]
        public DateTime final { get; set; }
        [Required]
        public int cantidad_personas { get; set; }

        [Required]
        public int tipo_solicitud { get; set; }
        [Required]
        public int usuario { get; set; }
        public string comentario { get; set; }

    }
}
