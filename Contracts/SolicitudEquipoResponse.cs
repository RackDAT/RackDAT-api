using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class SolicitudEquipoResponse
    {
        public EquipoResponse equipo { get; set; }
        public SolicitudResponse folio { get; set; }
        public DateTime salida { get; set; }
        public DateTime vuelta { get; set; }

    }
}
