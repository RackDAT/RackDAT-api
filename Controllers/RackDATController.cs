using Microsoft.AspNetCore.Mvc;
using RackDAT_API.Contracts;
using RackDAT_API.Models;
using Supabase;

namespace RackDAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RackDATController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;
        public RackDATController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        [HttpPost]
        public async Task<IActionResult> Ayuda(CreateCarreraRequest request)
        {
            var carrera = new Carreras
            {
                carrera = request.carrera
            };

            var response = await _supabaseClient.From<Carreras>().Insert(carrera);

            var newCarrera = response.Models.First();

            return Ok("Suputamadre soy un Dios");
        }

    }
}
