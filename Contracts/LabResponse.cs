using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class LabResponse
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public SalonResponse salon { get; set; }
    }
}
