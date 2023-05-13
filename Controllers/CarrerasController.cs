using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RackDAT_API.Contracts;
using RackDAT_API.Models;

namespace RackDAT_API.Controllers

{
    [Route("/[controller]")]
    [ApiController]
    public class CarrerasController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly HttpClient _httpClient;
        public CarrerasController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
            _httpClient = new HttpClient();
        }

        [HttpPost("carrera")]
        [Authorize]
        public async Task<IActionResult> postCarrera(CreateCarreraRequest request)
        {
            var carrera = new Carrera
            {
                carrera = request.carrera,
                siglas = request.siglas
            };

            var response = await _supabaseClient.From<Carrera>().Insert(carrera);

            var newCarrera = response.Models.First();

            return Ok(JsonConvert.SerializeObject(newCarrera));
        }


        [HttpGet("carrera/{id}")]
        public async Task<IActionResult> getCarreraID(int id)
        {
            var response = await _supabaseClient.From<Carrera>().Where(n => n.id == id).Get();
            var carrera = response.Models.FirstOrDefault();
            if (carrera is null)
            {
                return NotFound(null);
            }
            return Ok(JsonConvert.SerializeObject(carrera));
        }

        [HttpGet("carreras")]
        public async Task<ActionResult<IEnumerable<CarreraResponse>>> getCarrera()
        {
            var response = await _supabaseClient.From<Carrera>().Get();
            var carreras = response.Models;
            return Ok(JsonConvert.SerializeObject(carreras));
        }

    }
}
