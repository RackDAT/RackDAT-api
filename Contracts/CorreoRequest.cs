using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CorreoRequest
    {
        [Required]
        public string correo { get; set; }
    }
}
