using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateReactivoEstanteriaRequest
    {
        [Required]
        public int estanteria { get; set; }
        [Required]
        public int reactivo { get; set; }
        [Required]
        public float cantidad { get; set; }

    }
}
