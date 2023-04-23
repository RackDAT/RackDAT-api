using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class Salon : BaseModel
    {
        [PrimaryKey]
        public int id_salon { get; set; }
        [Required]
        public string salon { get; set; }
        [Required]
        public string description { get; set; }
    }
}
