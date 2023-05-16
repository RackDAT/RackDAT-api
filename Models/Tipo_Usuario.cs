using Postgrest.Attributes;
#pragma warning disable CS0105 // The using directive for 'Postgrest.Attributes' appeared previously in this namespace
using Postgrest.Attributes;
#pragma warning restore CS0105 // The using directive for 'Postgrest.Attributes' appeared previously in this namespace
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("tipo_usuario")]
    public class Tipo_Usuario :  BaseModel
    {
        [PrimaryKey("id_tipo_usuario", false)]
        public int id { get; set; }
        [Required, Column("tipo_usuario")]
#pragma warning disable CS8618 // Non-nullable property 'tipo_usuario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string tipo_usuario { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'tipo_usuario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
