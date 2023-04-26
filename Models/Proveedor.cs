﻿using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Models
{
    public class Proveedor : BaseModel
    {
        [PrimaryKey("id_proveedor", false)]
        public int id { get; set; }
        [Required]
        public string proveedor { get; set; }
    }
}
