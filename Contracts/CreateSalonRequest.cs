using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateSalonRequest
    {
        [Required]
        public string salon { get; set; }

        [Required]
        public string descripcion { get; set; }
        [Required]
        public string imagen { get; set; }

    }
}
