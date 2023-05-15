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
        [Column("descripcion")]
        public string descripcion { get; set; }
        [Column("fecha_compra")]
        public DateOnly fecha_compra { get; set; }
        [Column("tag")]
        public string tag { get; set; }

        [Column("id_modelo")]
        public int id_modelo { get; set; }

        [Reference(typeof(Modelo))]
        public Modelo modelo { get; set; }
        [Column("imagen")]
        public string imagen { get; set;}
        [Column("comentario")]
        public string comentario { get; set; }
        [Column("id_estanteria")]
        public int id_estanteria { get; set; }

        [Reference(typeof(Estanteria))]
        public Estanteria estanteria { get; set; }

    }
}
