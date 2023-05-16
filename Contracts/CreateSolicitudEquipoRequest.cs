using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateSolicitudEquipoRequest
    {
        [Required]
        public int[] equipos { get; set; }
        [Required]
        public int folio { get; set; }
        [Required]
        public DateTime salida { get; set; }
        [Required]
        public DateTime vuelta { get; set; }
        [Required]
        public int usuario { get; set; }
        public string comentario { get; set; }

    }
}
