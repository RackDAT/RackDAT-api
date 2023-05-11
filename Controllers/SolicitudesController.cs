
using Microsoft.AspNetCore.Mvc;
using Supabase;
using RackDAT_API.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace RackDAT_API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class SolicitudesController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly HttpClient _httpClient;
        public SolicitudesController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
            _httpClient = new HttpClient();
        }

        [HttpGet("GetSolicitudesPendientes")]
        public async Task<IActionResult> getSolicitudesPendientes()
        {
            var responseSolicitudesEquipo = await _supabaseClient.From<Solicitud_Equipo>().Get();
            var solicitudesEquipo = responseSolicitudesEquipo.Models;
            var responseSolicitudesLaboratorio = await _supabaseClient.From<Solicitud_Lab>().Get();
            var solicitudesLaboratorio = responseSolicitudesLaboratorio.Models;
            var response = new { equipos = solicitudesEquipo, laboratorios = solicitudesLaboratorio };

            var GroupedSolicitudesEquipos = new Dictionary<int, Solicitud_Equipo>();

            foreach (Solicitud_Equipo solicitud in solicitudesEquipo)
            {
                if (!GroupedSolicitudesEquipos.ContainsKey(solicitud.folio))
                {

                    GroupedSolicitudesEquipos.Add(solicitud.folio, solicitud);
                }
            }
            return Ok(JsonConvert.SerializeObject(GroupedSolicitudesEquipos.Values));
        }
    }
}
