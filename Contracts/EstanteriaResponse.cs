using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class EstanteriaResponse
    {
        public int id { get; set; }
        public string localidad { get; set; }
        public string lab { get; set; }
        public string color { get; set;
        }
    }

}
