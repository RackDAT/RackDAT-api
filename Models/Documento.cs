using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;


namespace RackDAT_API.Models
{
    public class Documento
    {
        [PrimaryKey("id_documento", false)]
        public int id_documento { get; set; }
        public string documento { get; set; }
        public int id_TiposDocumentos { get; set; }

    }
}
