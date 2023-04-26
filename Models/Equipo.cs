using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;


namespace RackDAT_API.Models
{
    public class Equipo
    {
        [PrimaryKey("id_equipo", false)]
        public int id { get; set; }
        [Required]
        public string ns { get; set; }
        public string description { get; set; }
        public DateOnly fecha_compra { get; set; }

        [Required]
        public string tag { get; set; }

        [Required]
        public int id_modelo { get; set; }

        public int id_comentario { get; set; }

    }
}
