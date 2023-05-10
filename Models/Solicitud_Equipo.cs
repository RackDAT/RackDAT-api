﻿using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;
using System.ComponentModel.DataAnnotations;
namespace RackDAT_API.Models
{
    [Table("solicitud_equipo")]
    public class Solicitud_Equipo : BaseModel
    {
        [PrimaryKey("id_solicitud", false)]
        public int folio { get; set; }
        [PrimaryKey("id_equipo", false)]
        public int equipo { get; set; }
        public DateTime fecha_salida { get; set; }
        public DateTime fecha_vuelta { get; set; }

    }
}
