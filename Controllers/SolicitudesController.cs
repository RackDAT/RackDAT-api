
using Microsoft.AspNetCore.Mvc;
using Supabase;
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
            var response = await _supabaseClient.From<Solicitud_Atributos>().Order(n => n.fecha_pedido, Postgrest.Constants.Ordering.Descending).Get();
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
        [HttpPost("solicitud/lab")] //publicar una solicitud de laboratorio
        public async Task<ActionResult> postSolicitudLab(CreateSolicitudLabRequest request)
        {
            var solicitud = new Solicitud
            {
                id_usuario = request.usuario,
                comentario = request.comentario,
                id_tipo_solicitud = 3,
                id_estatus_solicitud = 3,
                fecha_pedido = DateTime.Now
            };

            var response = await _supabaseClient.From<Solicitud>().Insert(solicitud);

            var newSolicitud = response.Models.First();

            var solicitud_lab = new Solicitud_Lab
            {
                id_solicitud = newSolicitud.folio,
                id_laboratorio = request.lab,
                fecha_salida = request.inicio,
                fecha_vuelta = request.final,
                cantidad_personas = request.cantidad_personas
            };

            var response_lab = await _supabaseClient.From<Solicitud_Lab>().Insert(solicitud_lab);
            var newSolicitud_lab = response_lab.Models;

            return Ok(JsonConvert.SerializeObject(newSolicitud_lab));
        }
        [HttpGet("solicitud/{id}")] //obtener solicitud por id
        public async Task<ActionResult> getSolicitudID(int id)
        {
            var response = await _supabaseClient.From<Solicitud_Atributos>().Where(x => x.folio == id).Get();
            var solicitud = response.Models.FirstOrDefault();
            return Ok(JsonConvert.SerializeObject(solicitud));
        }


        [HttpGet("solicitudes-historicas/carrera/{id}")] //solicitudes historicas de una carrera
        public async Task<ActionResult> getSolicitudesHistoricasCarrera(int id)
        {
            var response = await _supabaseClient.From<Solicitud_Atributos>().Where(n => n.carrera == id).Order(n => n.fecha_pedido, Postgrest.Constants.Ordering.Descending).Get();
            var solicitudes = response.Models;

            return Ok(JsonConvert.SerializeObject(solicitudes));

        }

        [HttpGet("solicitudes-pendientes/carrera/{id}")] //solicitudes historicas de una carrera
        public async Task<ActionResult> getSolicitudesPendientesCarrera(int id)
        {
            var response = await _supabaseClient.From<Solicitud_Atributos>().Where(n => n.id_estatus_solicitud == 3 && n.carrera == id).Order(n => n.fecha_pedido, Postgrest.Constants.Ordering.Descending).Get();
            var solicitudes = response.Models;

            return Ok(JsonConvert.SerializeObject(solicitudes));

        }

        [HttpPost("solicitud/equipo")] //publicar una solicitud de equipos
        public async Task<ActionResult> postSolicitudEquipo(CreateSolicitudEquipoRequest request)
        {
            var solicitud = new Solicitud
            {
                id_usuario = request.usuario,
                comentario = request.comentario,
                id_tipo_solicitud = 1,
                id_estatus_solicitud = 3,
                fecha_pedido = DateTime.Now
            };
            
            var response = await _supabaseClient.From<Solicitud>().Insert(solicitud);

            var newSolicitud = response.Models.First();

            List<Solicitud_Equipo> solicitudes_response = new List<Solicitud_Equipo>();
            foreach (int equipo in request.equipos)
            {
                var solicitud_equipo = new Solicitud_Equipo
                {
                    folio = newSolicitud.folio,
                    id_equipo = equipo,
                    fecha_salida = request.salida,
                    fecha_vuelta = request.vuelta
                };
                var response_equipo = await _supabaseClient.From<Solicitud_Equipo>().Insert(solicitud_equipo);
                var newSolicitud_equipo = response_equipo.Models.First();
                solicitudes_response.Add(newSolicitud_equipo);
                
            }


            return Ok(JsonConvert.SerializeObject(solicitudes_response));
        }

    }
}
