using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
#pragma warning disable CS0105 // The using directive for 'System.ComponentModel.DataAnnotations' appeared previously in this namespace
using System.ComponentModel.DataAnnotations;
#pragma warning restore CS0105 // The using directive for 'System.ComponentModel.DataAnnotations' appeared previously in this namespace
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
#pragma warning disable CS8618 // Non-nullable property 'ns' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string ns { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'ns' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Column("descripcion")]
#pragma warning disable CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string descripcion { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'descripcion' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Column("fecha_compra")]
        public DateOnly fecha_compra { get; set; }
        [Column("tag")]
#pragma warning disable CS8618 // Non-nullable property 'tag' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string tag { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'tag' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Column("id_modelo")]
        public int id_modelo { get; set; }

        [Reference(typeof(Modelo))]
#pragma warning disable CS8618 // Non-nullable property 'modelo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Modelo modelo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'modelo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Column("imagen")]
#pragma warning disable CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string imagen { get; set;}
#pragma warning restore CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Column("comentario")]
#pragma warning disable CS8618 // Non-nullable property 'comentario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string comentario { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'comentario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Column("id_estanteria")]
        public int id_estanteria { get; set; }

        [Reference(typeof(Estanteria))]
#pragma warning disable CS8618 // Non-nullable property 'estanteria' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Estanteria estanteria { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'estanteria' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    }
}
