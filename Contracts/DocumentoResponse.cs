using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class DocumentoResponse
    {
        public int id { get; set; }
        public string link { get; set; }
        public TipoDocumentoResponse tipo_doc { get; set;}
    }

}
