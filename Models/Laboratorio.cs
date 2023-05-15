using Microsoft.AspNetCore.Mvc;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    [Table("laboratorio")]
    public class Laboratorio : BaseModel
    {

        [PrimaryKey("id_laboratorio", false)]
        public int id { get; set; }
        [Required]
        public string laboratorio { get; set; }
        [Required, Column("id_salon")]
        public int id_salon { get; set; }

        [Reference(typeof(Salon))]
        public Salon salon { get; set; }
        [Required, Column("imagen")]
        public string imagen { get;set; }
        [Required, Column("descripcion_lab")]
        public string descripcion_lab { get; set; }
    }
}
