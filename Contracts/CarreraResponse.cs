using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class CarreraResponse
    {
        public int id { get; set; }
        public string carrera { get; set; }
        public string siglas { get; set; }
    }
}
