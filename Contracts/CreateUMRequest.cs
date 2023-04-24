using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateUMRequest
    {
        [Required]
        public string unidad_medida { get; set; }
    }
}
