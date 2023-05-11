using RackDAT_API.Models;
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


        [HttpGet("GetLaboratorios")]
        public async Task<IActionResult> getSolicitudesPendientes()
        {
            var response = await _supabaseClient.From<Laboratorio>().Get();
            return Ok(JsonConvert.SerializeObject(response.Models));
        }

        [HttpGet("GetLaboratorioById/{id}")]
        public async Task<IActionResult> getLaboratorioById(int id)
        {
            var response = await _supabaseClient.From<Laboratorio>().Where(x => x.id == id).Single();
            return Ok(JsonConvert.SerializeObject(response));
        }
    }
}
