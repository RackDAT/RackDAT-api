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
#pragma warning disable CS8618 // Non-nullable property 'laboratorio' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string laboratorio { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'laboratorio' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required, Column("id_salon")]
        public int id_salon { get; set; }

        [Reference(typeof(Salon))]
#pragma warning disable CS8618 // Non-nullable property 'salon' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public Salon salon { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'salon' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required, Column("imagen")]
#pragma warning disable CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string imagen { get;set; }
#pragma warning restore CS8618 // Non-nullable property 'imagen' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required, Column("descripcion_lab")]
#pragma warning disable CS8618 // Non-nullable property 'descripcion_lab' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string descripcion_lab { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'descripcion_lab' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
