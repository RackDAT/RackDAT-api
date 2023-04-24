using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateEquipoEstanteriaRequest
    {
        [Required]
        public int equipo { get; set; }
        [Required]
        public int estanteria { get; set;}

    }
}
