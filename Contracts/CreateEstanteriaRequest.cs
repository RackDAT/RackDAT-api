using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateEstanteriaRequest
    {
        [Required]
        public string localidad { get; set; }
        [Required]
        public int lab { get; set; }
        public string color { get; set; }

    }
}
