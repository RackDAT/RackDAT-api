using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateEquipoDocRequest
    {
        [Required]
        public int documento { get; set; }
        [Required]
        public int equipo { get; set; }

    }
}
