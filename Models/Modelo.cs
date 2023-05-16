using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("modelo")]
    public class Modelo : BaseModel
    {

        [PrimaryKey("id_modelo", false)]
        public int id { get; set; }
        [Required, Column("modelo")]
#pragma warning disable CS8618 // Non-nullable property 'modelo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string modelo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'modelo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required, Column("id_proveedor"), ]
        public int id_proveedor { get; set; }
        [Reference(typeof(Proveedor))]
#pragma warning disable CS8618 // Non-nullable property 'proveedor' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public  Proveedor proveedor { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'proveedor' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
