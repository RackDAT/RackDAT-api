using Microsoft.AspNetCore.Mvc;
using RackDAT_API.Models;
using RackDAT_API.Contracts;
using Newtonsoft.Json;
#pragma warning disable CS0105 // The using directive for 'Microsoft.AspNetCore.Mvc' appeared previously in this namespace
using Microsoft.AspNetCore.Mvc;
#pragma warning restore CS0105 // The using directive for 'Microsoft.AspNetCore.Mvc' appeared previously in this namespace
using System.Collections.Generic;
namespace RackDAT_API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EstanteriasController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly HttpClient _httpClient;

        public EstanteriasController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
            _httpClient = new HttpClient();
        }

        //-----------------------Estanterias Endpoints-------------------------------------------------------//
        [HttpPost("estanteria")]
        public async Task<IActionResult> postEstanteria(CreateEstanteriaRequest request)
        {
            var estanteria = new Estanteria
            {
                estanteria = request.localidad,
                id_laboratorio = request.lab,
                color = request.color
            };

            var response = await _supabaseClient.From<Estanteria>().Insert(estanteria);

            var newEstanteria = response.Models.First();

            return Ok(JsonConvert.SerializeObject(newEstanteria));
        }

        [HttpGet("estanteria/{id}")]
        public async Task<IActionResult> getEstanteriaID(int id)
        {
            var response = await _supabaseClient.From<Estanteria>().Where(n => n.id == id).Get();
            var estanteria = response.Models.FirstOrDefault();
            
            return Ok(JsonConvert.SerializeObject(estanteria));
        }

        [HttpGet("estanterias")]
        public async Task<IActionResult> getEstanterias()
        {
            var response = await _supabaseClient.From<Estanteria>().Get();
            var estanteriaContenido = response.Models;
            
            return Ok(JsonConvert.SerializeObject(estanteriaContenido));
        }
    }
}
