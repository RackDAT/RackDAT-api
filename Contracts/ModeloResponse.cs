using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class ModeloResponse
    {
        public int id { get; set; }
        public string modelo { get; set; }
        public ProveedorResponse proveedor { get; set; }
    }

}
