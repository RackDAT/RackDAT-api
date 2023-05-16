using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;


namespace RackDAT_API.Models
{

    [Table("carrera")]
    public class Carrera : BaseModel
    {
        [PrimaryKey("id_carrera", false)]
        public int id { get; set; }


        [Column("carrera")]
#pragma warning disable CS8618 // Non-nullable property 'carrera' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string carrera { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'carrera' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.


        [Column("siglas")]
#pragma warning disable CS8618 // Non-nullable property 'siglas' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string siglas { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'siglas' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }

}
