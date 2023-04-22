using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateReactivoRequest
    {
        [Required]
        public string nombre { get; set; }
        [Required]
        public int medida { get; set; }

    }
}
