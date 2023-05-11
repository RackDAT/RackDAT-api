using Microsoft.AspNetCore.Mvc;
using RackDAT_API.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;



namespace RackDAT_API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EquiposController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly HttpClient _httpClient;

        public EquiposController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
            _httpClient = new HttpClient();
        }

        [HttpGet("GetEquipos")]
        public async Task<IActionResult> getSolicitudesPendientes()
        {
            var response = await _supabaseClient.From<Equipo>().Get();

            return Ok(JsonConvert.SerializeObject(response.Models));
        }
    }
}
