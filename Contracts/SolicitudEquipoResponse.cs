using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class SolicitudEquipoResponse
    {
        public int equipo { get; set; }
        public int folio { get; set; }
        public DateTime salida { get; set; }
        public DateTime vuelta { get; set; }

    }
}
