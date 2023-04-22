using Microsoft.AspNetCore.Mvc;
using RackDAT_API.Contracts;
using RackDAT_API.Models;
using Supabase;
using System.Collections;

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
                nombre = request.nombre
            };

            var response = await _supabaseClient.From<Carreras>().Insert(carrera);

            var newCarrera = response.Models.First();

            var carreraResponse = new CarreraResponse
            {
                id = newCarrera.id,
                carrera = newCarrera.nombre
            };

            return Ok(carreraResponse);
        }
        [HttpGet("id:int")]
        public async Task<IActionResult> Damelo(int id)
        {
            var response = await _supabaseClient.From<Carreras>().Where(n=>n.id == id).Get();
            var carrera = response.Models.FirstOrDefault();
            if(carrera is null)
            {
                return NotFound("Watafac");
            }
            var carreraResponse = new CarreraResponse
            {
                id = carrera.id,
                carrera = carrera.nombre
            };
            return Ok(carreraResponse);
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<CarreraResponse>>> DameloTodo()
        {
            var response = await _supabaseClient.From<Carreras>().Get();
            var carrerasR = response.Models;
            if (carrerasR is null)
            {
                return NotFound();
            }
            List<CarreraResponse> regresar = new List<CarreraResponse>();
            foreach(Carreras carrera in carrerasR)
            {
                regresar.Add(new CarreraResponse 
                    { 
                        id = carrera.id,
                        carrera = carrera.nombre 
                    }
                );
            }
            return Ok(regresar);
        }



    }
}
