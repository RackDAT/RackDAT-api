using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class ModelosResponse
    {
        public int id { get; set; }
        public string modelo { get; set; }
        public ProveedoresResponse proveedor { get; set; }
    }

}
