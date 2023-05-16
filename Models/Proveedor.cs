using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("proveedor")]
    public class Proveedor : BaseModel
    {
        [PrimaryKey("id_proveedor", false)]
        public int id { get; set; }
        [Required, Column("proveedor")]
#pragma warning disable CS8618 // Non-nullable property 'proveedor' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string proveedor { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'proveedor' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
