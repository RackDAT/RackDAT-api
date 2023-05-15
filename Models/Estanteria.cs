using System.Drawing;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("estanteria")]
    public class Estanteria : BaseModel
    {
        [PrimaryKey("id_estanteria", false)]
        public int id { get; set; }
        [Required, Column("estanteria")]
        public string estanteria { get; set; }
        [Column("color")]
        public string? color { get; set; }

        [Required, Column("id_laboratorio")]
        public int id_laboratorio { get; set; }
        [Reference(typeof(Laboratorio))]
        public Laboratorio laboratorio { get; set; }
    }
}
