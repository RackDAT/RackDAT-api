using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateCarreraRequest
    {
        [Required]
        public string carrera { get; set; }
        [Required]
        public string siglas { get; set;}

    }

}
