using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateSolicitudReactivoRequest
    {
        [Required]
        public int reactivo { get; set; }
        [Required]
        public int folio { get; set; }
        [Required]
        public float cantidad { get; set; }
        [Required]
        public DateOnly fecha_salida { get; set; }

    }
}
