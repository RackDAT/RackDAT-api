
using Microsoft.AspNetCore.Mvc;
using Supabase;
using RackDAT_API.Models;
using RackDAT_API.Contracts;
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
        //-------------------------------------------------------------------------------//

        [HttpPost("solicitud")] //crear una solicitud
        public async Task<IActionResult> postSolicitud(CreateSolicitudRequest request)
        {

            var solicitud = new Solicitud
            {
                id_usuario = request.usuario,
                comentario = request.comentario,
                id_tipo_solicitud = request.tipo_solicitud,
                id_estatus_solicitud = 3,
                fecha_pedido = DateTime.Now
            };

            var response = await _supabaseClient.From<Solicitud>().Insert(solicitud);

            var newSolicitud = response.Models.First();

            return Ok(JsonConvert.SerializeObject(newSolicitud));
        }

        [HttpGet("solicitudes-pendientes")] //solicitudes pendientes de todas las personas
        public async Task<IActionResult> getSolicitudesPendientes()
        {
            var response = await _supabaseClient.From<Solicitud_Atributos>().Where(x => x.id_estatus_solicitud == 3).Get();

            var solicitudes = response.Models;
            return Ok(JsonConvert.SerializeObject(solicitudes));
        }

        [HttpGet("solicitudes-historicas")] //solicitudes historicas de todos las personas
        public async Task<ActionResult> getSolicitudesHistoricas()
        {
            var response = await _supabaseClient.From<Solicitud_Atributos>().Where(n => n.id_estatus_solicitud != 3).Order(n => n.fecha_pedido, Postgrest.Constants.Ordering.Descending).Get();
            var solicitudes = response.Models;

            return Ok(JsonConvert.SerializeObject(solicitudes));

        }


        [HttpPatch("solicitud-verificar/{id}")] //aceptar o rechazar solicitud
        public async Task<ActionResult> verificarSolicitud(int id, bool verificacion, int usuarioID)
        {

            var usuario_contenido = await _supabaseClient.From<Usuario>().Where(n => n.id == usuarioID).Get();
            var usuario = usuario_contenido.Models.FirstOrDefault();
            if(usuario == null)
            {
                ActionResult GetTeapot = new ObjectResult("I'm a teapot")
                {
                    StatusCode = 418
                };
                return GetTeapot;
            }

            if (usuario.tipo_usuario.id == 3)
            {
                await _supabaseClient.From<Solicitud>().Where(n => n.folio == id).Set(x => x.aprobacion_tecnico, verificacion).Update();
            }
            else if (usuario.tipo_usuario.id == 4)
            {
                await _supabaseClient.From<Solicitud>().Where(n => n.folio == id).Set(x => x.aprobacion_coordinador, verificacion).Update();
            }
            else
            {
                ActionResult GetTeapot = new ObjectResult("I'm a teapot")
                {
                    StatusCode = 418
                };
                return GetTeapot;
            }

            var response = await _supabaseClient.From<Solicitud>().Where(n => n.folio == id).Get();
            var solicitud = response.Models.FirstOrDefault();

            return Ok(JsonConvert.SerializeObject(solicitud));
        }

    }
}
