using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateModeloRequest
    {
        [Required]
        public string modelo { get; set; }
        [Required]
        public int proveedor { get; set; }

    }
}
