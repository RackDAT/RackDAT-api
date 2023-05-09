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
        public int salon { get; set; }
        [Required]
        public string imagen { get;set; }
        [Required]
        public string descripcion_lab { get; set; }
    }
}
