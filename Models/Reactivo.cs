using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class Reactivo : BaseModel
    {
        [PrimaryKey("id_reactivo", false)]
#pragma warning disable CS8618 // Non-nullable property 'id' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string id { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'id' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'reactivo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string reactivo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'reactivo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Required]
        public int id_unidad_medida { get; set; }
    }
}
