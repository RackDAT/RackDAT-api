namespace RackDAT_API.Contracts
{
    public class EquipoDocsResponse
    {
#pragma warning disable CS8618 // Non-nullable property 'documento' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public DocumentoResponse documento { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'documento' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

#pragma warning disable CS8618 // Non-nullable property 'equipo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public EquipoResponse equipo { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'equipo' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    }
}
