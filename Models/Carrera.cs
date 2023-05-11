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
        public string carrera { get; set; }


        [Column("siglas")]
        public string siglas { get; set; }
    }

}
