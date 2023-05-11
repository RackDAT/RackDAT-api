using Microsoft.AspNetCore.Mvc;
using RackDAT_API.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;



namespace RackDAT_API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly HttpClient _httpClient;

        public UsuariosController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
            _httpClient = new HttpClient();
        }

        [HttpGet("GetUsuariosVerificados")]
        public async Task<IActionResult> getUsuariosVerificados()
        {
            var response = await _supabaseClient.From<Usuario>().Where(x => x.verificado == true).Get();

            return Ok(JsonConvert.SerializeObject(response.Models));
        }

    }
}
