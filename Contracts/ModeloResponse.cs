using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class ModeloResponse
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'modelo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string modelo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'modelo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'proveedor' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public ProveedorResponse proveedor { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'proveedor' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }

}
