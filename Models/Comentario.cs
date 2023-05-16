using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
#pragma warning disable CS0105 // The using directive for 'System.ComponentModel.DataAnnotations' appeared previously in this namespace
using System.ComponentModel.DataAnnotations;
#pragma warning restore CS0105 // The using directive for 'System.ComponentModel.DataAnnotations' appeared previously in this namespace

namespace RackDAT_API.Models
{
    public class Comentario : BaseModel
    {
        [Required]
#pragma warning disable CS8618 // Non-nullable property 'comentario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string comentario { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'comentario' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    }
}
