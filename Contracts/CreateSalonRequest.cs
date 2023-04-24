using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateSalonRequest
    {
        [Required]
        public string nombre { get; set; }

        [Required]
        public string descripcion { get; set; }

    }
}
