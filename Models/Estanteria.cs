using System.Drawing;
using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class Estanteria
    {

        public int id_estanteria { get; set; }
        [Required]
        public string estanteria { get; set; }
        public string color { get; set; }

        [Required]
        public int id_laboratorio { get; set; }
    }
}
