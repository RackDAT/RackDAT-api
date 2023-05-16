using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class SolicitudEquipoResponse
    {
#pragma warning disable CS8618 // Non-nullable property 'equipo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public EquipoResponse equipo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'equipo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'folio' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public SolicitudResponse folio { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'folio' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public DateTime salida { get; set; }
        public DateTime vuelta { get; set; }

    }
}
