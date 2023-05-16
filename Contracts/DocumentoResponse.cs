using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class DocumentoResponse
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'link' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string link { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'link' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'tipo_doc' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public TipoDocumentoResponse tipo_doc { get; set;}
#pragma warning restore CS8618 // Non-nullable property 'tipo_doc' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }

}
