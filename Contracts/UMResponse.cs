using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class UMResponse
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'unidad_medida' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string unidad_medida { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'unidad_medida' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
