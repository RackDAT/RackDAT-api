using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts;

public class CreateDocumentoRequest
{
    [Required]
#pragma warning disable CS8618 // Non-nullable property 'link' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    public string link { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'link' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    [Required]
    public int tipo_doc { get; set; }

}
