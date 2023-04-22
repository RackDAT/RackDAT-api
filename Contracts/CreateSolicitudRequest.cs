using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateSolicitudRequest
    {
        [Required]
        public DateTime fecha_actualizacion { get; set; }
        [Required]
        public int tipo_Solicitud { get; set; }
        [Required]
        public int usuario { get; set; }
        [Required]
        public int estatus { get; set; }
        public string comentarios { get; set; }

    }
}
