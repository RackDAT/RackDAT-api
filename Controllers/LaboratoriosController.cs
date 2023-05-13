using RackDAT_API.Models;
using RackDAT_API.Contracts;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace RackDAT_API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LaboratoriosController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly HttpClient _httpClient;

        public LaboratoriosController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
            _httpClient = new HttpClient();
        }
        [HttpPost("lab")]
        public async Task<IActionResult> postLab(CreateLabRequest request)
        {
            var laboratorio = new Laboratorio
            {
                laboratorio = request.lab,
                salon = request.salon,
                imagen = request.imagen,
                descripcion_lab = request.descripcion

            };

            var response = await _supabaseClient.From<Laboratorio>().Insert(laboratorio);

            var newLab = response.Models.First();

            return Ok(JsonConvert.SerializeObject(newLab));
        }
        [HttpGet("labs")]
        public async Task<ActionResult<IEnumerable<LabResponse>>> getLab()
        {
            var response = await _supabaseClient.From<Laboratorio>().Get();
            var labs = response.Models;

            return Ok(JsonConvert.SerializeObject(labs));
        }

        [HttpGet("lab/{id}")]
        public async Task<ActionResult<LabResponse>> getLabID(int id)
        {
            var response = await _supabaseClient.From<Laboratorio>().Where(n => n.id == id).Get();
            var lab = response.Models.FirstOrDefault();
            
            return Ok(JsonConvert.SerializeObject(lab));
        }
    }
}
