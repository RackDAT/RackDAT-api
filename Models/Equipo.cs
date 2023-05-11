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
        [Column("ns")]
        public string ns { get; set; }
        public string descripcion { get; set; }
        public DateOnly fecha_compra { get; set; }

        public string tag { get; set; }

         [Column("id_modelo")]
        public int modelo { get; set; }
        public string imagen { get; set;}
        public string comentario { get; set; }
        public int id_estanteria { get; set; }

    }
}
