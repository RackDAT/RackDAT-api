using System.Drawing;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
#pragma warning disable CS0105 // The using directive for 'System.ComponentModel.DataAnnotations' appeared previously in this namespace
using System.ComponentModel.DataAnnotations;
#pragma warning restore CS0105 // The using directive for 'System.ComponentModel.DataAnnotations' appeared previously in this namespace

namespace RackDAT_API.Models
{
    [Table("estanteria")]
    public class Estanteria : BaseModel
    {
        [PrimaryKey("id_estanteria", false)]
        public int id { get; set; }
        [Required, Column("estanteria")]
#pragma warning disable CS8618 // Non-nullable property 'estanteria' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string estanteria { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'estanteria' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Column("color")]
        public string? color { get; set; }

        [Required, Column("id_laboratorio")]
        public int id_laboratorio { get; set; }
        [Reference(typeof(Laboratorio))]
#pragma warning disable CS8618 // Non-nullable property 'laboratorio' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Laboratorio laboratorio { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'laboratorio' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
