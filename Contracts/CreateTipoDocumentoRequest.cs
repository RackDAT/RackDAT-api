using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateTipoDocumentoRequest
    {
        [Required]
        public string nombre { get; set; }

    }
}
