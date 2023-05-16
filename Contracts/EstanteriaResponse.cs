using System.ComponentModel.DataAnnotations;

namespace RackDAT_API.Contracts
{
    public class EstanteriaResponse
    {
        public int id { get; set; }
#pragma warning disable CS8618 // Non-nullable property 'localidad' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string localidad { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'localidad' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'lab' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public LabResponse lab { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'lab' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'color' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string color { get; set;
#pragma warning restore CS8618 // Non-nullable property 'color' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        }
    }

}
