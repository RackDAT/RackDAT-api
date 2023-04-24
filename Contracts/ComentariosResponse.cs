using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class ComentariosResponse
    {
        public int id { get; set; }
        public string comentario { get; set; }
    }
}
