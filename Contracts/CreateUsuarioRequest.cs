using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CreateUsuarioRequest
    {
        [Required]
        public string nombre { get; set; }
        [Required]
        public string apellido_pat { get; set; }
        public string apellido_mat { get; set; }
        [Required]
        public string correo { get; set; }
        [Required]
        public string clave { get; set; }
        [Required]
        public int tipo_usuario { get; set; }
        [Required]
        public int carrera { get; set; }

    }
}
