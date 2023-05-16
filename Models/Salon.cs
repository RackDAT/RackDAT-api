using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
#pragma warning disable CS0105 // The using directive for 'System.ComponentModel.DataAnnotations' appeared previously in this namespace
using System.ComponentModel.DataAnnotations;
#pragma warning restore CS0105 // The using directive for 'System.ComponentModel.DataAnnotations' appeared previously in this namespace

namespace RackDAT_API.Models
{
    [Table("salon")]
    public class Salon : BaseModel
    {
        [PrimaryKey("id_salon", false)]
        public int id { get; set; }
        [Required, Column("salon")]
#pragma warning disable CS8618 // Non-nullable property 'salon' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string salon { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'salon' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required, Column("descripcion")]
#pragma warning disable CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string descripcion { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
