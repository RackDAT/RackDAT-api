using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateLabRequest
    {
        [Required]
        public string lab { get; set; }
        [Required]
        public int salon { get; set; }

    }
}
