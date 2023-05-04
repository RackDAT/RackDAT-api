using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;

namespace RackDAT_API.Models
{
    [Table("equipo")]
    public class Equipo : BaseModel
    {
        [PrimaryKey("id_equipo", false)]
        public int id { get; set; }
        [Required]
        public string ns { get; set; }
        [Required]
        public string descripcion { get; set; }
        [Required]
        public DateOnly fecha_compra { get; set; }

        [Required]
        public string tag { get; set; }

        [Required, Column("id_modelo")]
        public int modelo { get; set; }
        [Required]
        public string imagen { get; set;}
        [AllowNull]
        public string comentario { get; set; }

    }
}
