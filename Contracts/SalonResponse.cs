﻿using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class SalonResponse
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }
}