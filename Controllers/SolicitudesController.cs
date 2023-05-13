
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

        [HttpGet("GetSolicitudesPendientes")]
        public async Task<IActionResult> getSolicitudesPendientesB()
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
                aprobacion_tecnico = 3,
                aprobacion_coordinador = 3,
                fecha_pedido = DateTime.Now
            };

            var response = await _supabaseClient.From<Solicitud>().Insert(solicitud);

            var newSolicitud = response.Models.First();

            return Ok(JsonConvert.SerializeObject(newSolicitud));
        }

        [HttpGet("solicitudes-pendientes")] //solicitudes pendientes de todas las personas
        public async Task<IActionResult> getSolicitudesPendientes()
        {
            var response = await _supabaseClient.From<Solicitud>().Where(n => n.id_estatus_solicitud == 3).Get();
            var sol_equipoContenido = response.Models;

            List<SolicitudResponse> sol_equipoResponse = new List<SolicitudResponse>();
            foreach (Solicitud solicitud in sol_equipoContenido)
            {
                TipoSolicitudResponse tipo_solicitud;
                HttpResponseMessage tipoSolicitud_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/tipo-solicitud/id:int?id=" + solicitud.id_tipo_solicitud);
                string tipoSolicitudcontenido = await tipoSolicitud_res.Content.ReadAsStringAsync();
                tipo_solicitud = JsonConvert.DeserializeObject<TipoSolicitudResponse>(tipoSolicitudcontenido);
                if (tipo_solicitud == null)
                {
                    return BadRequest("Hubo un error al recibir el tipo de solicitud");
                }

                EstatusResponse estatus;
                HttpResponseMessage estatus_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/estatus-solicitud/id:int?id=" + solicitud.id_estatus_solicitud);
                string estatus_contenido = await estatus_res.Content.ReadAsStringAsync();
                estatus = JsonConvert.DeserializeObject<EstatusResponse>(estatus_contenido);
                if (estatus == null)
                {
                    return BadRequest("Hubo un error al recibir el estatus");
                }

                UsuarioResponse usuario;
                HttpResponseMessage usuario_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/usuario/id:int?id=" + solicitud.id_usuario);
                string usuario_contenido = await usuario_res.Content.ReadAsStringAsync();
                usuario = JsonConvert.DeserializeObject<UsuarioResponse>(usuario_contenido);
                if (usuario == null)
                {
                    return BadRequest("Hubo un error al recibir el usuario");
                }
                var folio = solicitud.folio;
                var tipo_solicitud_int = solicitud.id_tipo_solicitud;

                var imagen_contenido = await _supabaseClient.Rpc("obtener_imagen", new Dictionary<string, object> { { "folio_input", folio }, { "tipo_solicitud_input", tipo_solicitud_int } });
                var imagen_response = imagen_contenido.Content.Trim('"');

                var cantidad_equipos = 0;

                var nombre_lab = "";

                LabResponse lab = new LabResponse { };

                if (solicitud.id_tipo_solicitud == 1)
                {
                    var cantidad_equipos_contenido = await _supabaseClient.Rpc("obtener_cantidad", new Dictionary<string, object> { { "folio_input", folio } });
                    cantidad_equipos = int.Parse(cantidad_equipos_contenido.Content.Trim('"'));
                }
                else if (solicitud.id_tipo_solicitud == 3)
                {
                    var lab_id = await _supabaseClient.Rpc("obtener_lab", new Dictionary<string, object> { { "folio_input", folio } });
                    HttpResponseMessage lab_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/lab/id:int?id=" + lab_id);
                    string lab_contenido = await lab_res.Content.ReadAsStringAsync();
                    lab = JsonConvert.DeserializeObject<LabResponse>(lab_contenido);
                    if (lab == null)
                    {
                        return BadRequest("Hubo un error al recibir el laboratorio");
                    }
                }

                sol_equipoResponse.Add(new SolicitudResponse
                {
                    id = folio,
                    fecha_pedido = solicitud.fecha_pedido,
                    comentario = solicitud.comentario,
                    imagen_muestra = imagen_response,
                    tipo_solicitud = tipo_solicitud,
                    estatus = estatus,
                    usuario = usuario,
                    cantidad_equipos = cantidad_equipos,
                    lab = lab
                }
                );
            }
            return Ok(sol_equipoResponse);
        }

        [HttpGet("solicitudes-historicas")] //solicitudes historicas de todos las personas
        public async Task<ActionResult> getSolicitudesHistoricas()
        {
            var response = await _supabaseClient.From<Solicitud>().Where(n => n.id_estatus_solicitud != 3).Order(n => n.fecha_pedido, Postgrest.Constants.Ordering.Descending).Get();
            var sol_equipoContenido = response.Models;
            if (sol_equipoContenido is null)
            {

                return NotFound("No hay solicitudes de equipos por desplegar");
            }
            var sol_equipos = JsonConvert.DeserializeObject(sol_equipoContenido);
            foreach (var solicitud in sol_equipos)
            {
                var folio = solicitud;
                var tipo_solicitud_int = solicitud.id_tipo_solicitud;

                var imagen_contenido = await _supabaseClient.Rpc("obtener_imagen", new Dictionary<string, object> { { "folio_input", folio }, { "tipo_solicitud_input", tipo_solicitud_int } });
                var imagen_response = imagen_contenido.Content.Trim('"');

                var cantidad_equipos = 0;

                var nombre_lab = "";
            }

            //---------------------------------------------
            List<SolicitudResponse> sol_equipoResponse = new List<SolicitudResponse>();
            foreach (Solicitud solicitud in sol_equipoContenido)
            {
                TipoSolicitudResponse tipo_solicitud;
                HttpResponseMessage tipoSolicitud_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/tipo-solicitud/id:int?id=" + solicitud.id_tipo_solicitud);
                string tipoSolicitudcontenido = await tipoSolicitud_res.Content.ReadAsStringAsync();
                tipo_solicitud = JsonConvert.DeserializeObject<TipoSolicitudResponse>(tipoSolicitudcontenido);
                if (tipo_solicitud == null)
                {
                    return BadRequest("Hubo un error al recibir el tipo de solicitud");
                }

                EstatusResponse estatus;
                HttpResponseMessage estatus_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/estatus-solicitud/id:int?id=" + solicitud.id_estatus_solicitud);
                string estatus_contenido = await estatus_res.Content.ReadAsStringAsync();
                estatus = JsonConvert.DeserializeObject<EstatusResponse>(estatus_contenido);
                if (estatus == null)
                {
                    return BadRequest("Hubo un error al recibir el estatus");
                }

                UsuarioResponse usuario;
                HttpResponseMessage usuario_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/usuario/id:int?id=" + solicitud.id_usuario);
                string usuario_contenido = await usuario_res.Content.ReadAsStringAsync();
                usuario = JsonConvert.DeserializeObject<UsuarioResponse>(usuario_contenido);
                if (usuario == null)
                {
                    return BadRequest("Hubo un error al recibir el usuario");
                }

                var folio = solicitud.folio;
                var tipo_solicitud_int = solicitud.id_tipo_solicitud;

                var imagen_contenido = await _supabaseClient.Rpc("obtener_imagen", new Dictionary<string, object> { { "folio_input", folio }, { "tipo_solicitud_input", tipo_solicitud_int } });
                var imagen_response = imagen_contenido.Content.Trim('"');

                var cantidad_equipos = 0;

                var nombre_lab = "";

                LabResponse lab = new LabResponse { };

                if (solicitud.id_tipo_solicitud == 1)
                {
                    var cantidad_equipos_contenido = await _supabaseClient.Rpc("obtener_cantidad", new Dictionary<string, object> { { "folio_input", folio } });
                    cantidad_equipos = int.Parse(cantidad_equipos_contenido.Content.Trim('"'));
                }
                else if (solicitud.id_tipo_solicitud == 3)
                {
                    var lab_id = await _supabaseClient.Rpc("obtener_lab", new Dictionary<string, object> { { "folio_input", folio } });
                    HttpResponseMessage lab_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/lab/id:int?id=" + lab_id);
                    string lab_contenido = await lab_res.Content.ReadAsStringAsync();
                    lab = JsonConvert.DeserializeObject<LabResponse>(lab_contenido);
                    if (lab == null)
                    {
                        return BadRequest("Hubo un error al recibir el laboratorio");
                    }
                }

                sol_equipoResponse.Add(new SolicitudResponse
                {
                    id = folio,
                    fecha_pedido = solicitud.fecha_pedido,
                    comentario = solicitud.comentario,
                    imagen_muestra = imagen_response,
                    tipo_solicitud = tipo_solicitud,
                    estatus = estatus,
                    usuario = usuario,
                    cantidad_equipos = cantidad_equipos,
                    lab = lab
                }
                );
            }
            return Ok(sol_equipoResponse);

        }

        [HttpPatch("solicitud-verificar/{id}")] //aceptar o rechazar solicitud
        public async Task<ActionResult> verificarSolicitud(int id, bool verificacion, int usuarioID)
        {
            var newEstatus = 3;
            if (verificacion)
            {
                newEstatus = 4;
            }
            else
            {
                newEstatus = 7;
            }

            UsuarioResponse usuario;
            HttpResponseMessage usuario_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/usuario/id:int?id=" + usuarioID);
            string usuarioContenido = await usuario_res.Content.ReadAsStringAsync();
            usuario = JsonConvert.DeserializeObject<UsuarioResponse>(usuarioContenido);
            if (usuario == null)
            {
                return BadRequest("Hubo un error al recibir el tipo de solicitud");
            }


            if (usuario.tipo_usuario.id == 3)
            {
                await _supabaseClient.From<Solicitud>().Where(n => n.folio == id).Set(x => x.aprobacion_tecnico, newEstatus).Update();
            }
            else if (usuario.tipo_usuario.id == 4)
            {
                await _supabaseClient.From<Solicitud>().Where(n => n.folio == id).Set(x => x.aprobacion_coordinador, newEstatus).Update();
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
            if (solicitud is null)
            {
                return BadRequest("Hubo un error");
            }
            TipoSolicitudResponse tipo_solicitud;
            HttpResponseMessage tipoSolicitud_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/tipo-solicitud/id:int?id=" + solicitud.id_tipo_solicitud);
            string tipoSolicitudcontenido = await tipoSolicitud_res.Content.ReadAsStringAsync();
            tipo_solicitud = JsonConvert.DeserializeObject<TipoSolicitudResponse>(tipoSolicitudcontenido);
            if (tipo_solicitud == null)
            {
                return BadRequest("Hubo un error al recibir el tipo de solicitud");
            }

            EstatusResponse estatus;
            HttpResponseMessage estatus_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/estatus-solicitud/id:int?id=" + solicitud.id_estatus_solicitud);
            string estatus_contenido = await estatus_res.Content.ReadAsStringAsync();
            estatus = JsonConvert.DeserializeObject<EstatusResponse>(estatus_contenido);
            if (estatus == null)
            {
                return BadRequest("Hubo un error al recibir el estatus");
            }

            var folio = solicitud.folio;
            var tipo_solicitud_int = solicitud.id_tipo_solicitud;

            var imagen_contenido = await _supabaseClient.Rpc("obtener_imagen", new Dictionary<string, object> { { "folio_input", folio }, { "tipo_solicitud_input", tipo_solicitud_int } });
            var imagen_response = imagen_contenido.Content.Trim('"');

            var cantidad_equipos = 0;

            var nombre_lab = "";

            LabResponse lab = new LabResponse { };

            if (solicitud.id_tipo_solicitud == 1)
            {
                var cantidad_equipos_contenido = await _supabaseClient.Rpc("obtener_cantidad", new Dictionary<string, object> { { "folio_input", folio } });
                cantidad_equipos = int.Parse(cantidad_equipos_contenido.Content.Trim('"'));
            }
            else if (solicitud.id_tipo_solicitud == 3)
            {
                var lab_id = await _supabaseClient.Rpc("obtener_lab", new Dictionary<string, object> { { "folio_input", folio } });
                HttpResponseMessage lab_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/lab/id:int?id=" + lab_id.Content);
                string lab_contenido = await lab_res.Content.ReadAsStringAsync();
                lab = JsonConvert.DeserializeObject<LabResponse>(lab_contenido);
                if (lab == null)
                {
                    return BadRequest("Hubo un error al recibir el laboratorio");
                }
            }

            var solicitudResponse = new SolicitudResponse
            {
                id = folio,
                fecha_pedido = solicitud.fecha_pedido,
                comentario = solicitud.comentario,
                imagen_muestra = imagen_response,
                tipo_solicitud = tipo_solicitud,
                estatus = estatus,
                usuario = usuario,
                cantidad_equipos = cantidad_equipos,
                lab = lab
            };
            return Ok(solicitudResponse);
        }

        [HttpGet("tarjeta/solicitud/id:int")] //Obtener una solicitud junto con sus atributo formato de tarjeta
        public async Task<ActionResult> getTarjetaSolicitudID(int id)
        {
            var response = await _supabaseClient.From<Solicitud>().Where(n => n.folio == id).Get();
            var solicitud = response.Models.FirstOrDefault();
            if (solicitud is null)
            {
                return BadRequest("Hubo un error");
            }
            TipoSolicitudResponse tipo_solicitud;
            HttpResponseMessage tipoSolicitud_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/tipo-solicitud/id:int?id=" + solicitud.id_tipo_solicitud);
            string tipoSolicitudcontenido = await tipoSolicitud_res.Content.ReadAsStringAsync();
            tipo_solicitud = JsonConvert.DeserializeObject<TipoSolicitudResponse>(tipoSolicitudcontenido);
            if (tipo_solicitud == null)
            {
                return BadRequest("Hubo un error al recibir el tipo de solicitud");
            }

            EstatusResponse estatus;
            HttpResponseMessage estatus_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/estatus-solicitud/id:int?id=" + solicitud.id_estatus_solicitud);
            string estatus_contenido = await estatus_res.Content.ReadAsStringAsync();
            estatus = JsonConvert.DeserializeObject<EstatusResponse>(estatus_contenido);
            if (estatus == null)
            {
                return BadRequest("Hubo un error al recibir el estatus");
            }

            UsuarioResponse usuario;
            HttpResponseMessage usuario_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/usuario/id:int?id=" + solicitud.id_usuario);
            string usuario_contenido = await usuario_res.Content.ReadAsStringAsync();
            usuario = JsonConvert.DeserializeObject<UsuarioResponse>(usuario_contenido);
            if (usuario == null)
            {
                return BadRequest("Hubo un error al recibir el usuario");
            }

            var folio = solicitud.folio;
            var tipo_solicitud_int = solicitud.id_tipo_solicitud;

            var imagen_contenido = await _supabaseClient.Rpc("obtener_imagen", new Dictionary<string, object> { { "folio_input", folio }, { "tipo_solicitud_input", tipo_solicitud_int } });
            var imagen_response = imagen_contenido.Content.Trim('"');

            var cantidad_equipos = 0;

            LabResponse lab = new LabResponse { };

            if (solicitud.id_tipo_solicitud == 1)
            {
                var cantidad_equipos_contenido = await _supabaseClient.Rpc("obtener_cantidad", new Dictionary<string, object> { { "folio_input", folio } });
                cantidad_equipos = int.Parse(cantidad_equipos_contenido.Content.Trim('"'));
            }
            else if (solicitud.id_tipo_solicitud == 3)
            {
                var lab_id = await _supabaseClient.Rpc("obtener_lab", new Dictionary<string, object> { { "folio_input", folio } });
                HttpResponseMessage lab_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/lab/id:int?id=" + lab_id.Content);
                string lab_contenido = await lab_res.Content.ReadAsStringAsync();
                lab = JsonConvert.DeserializeObject<LabResponse>(lab_contenido);
                Console.WriteLine(lab_id);
                if (lab == null)
                {
                    return BadRequest("Hubo un error al recibir el laboratorio");
                }
            }

            var solicitudResponse = new SolicitudResponse
            {
                id = folio,
                fecha_pedido = solicitud.fecha_pedido,
                comentario = solicitud.comentario,
                imagen_muestra = imagen_response,
                tipo_solicitud = tipo_solicitud,
                estatus = estatus,
                usuario = usuario,
                cantidad_equipos = cantidad_equipos,
                lab = lab
            };
            return Ok(solicitudResponse);
        }

        [HttpGet("solicitud/id:int")] //Obtener una solicitud junto con sus atributo -------------------------------------------------------
        public async Task<ActionResult> getSolicitudID(int id)
        {
            var response = await _supabaseClient.From<Solicitud>().Where(n => n.folio == id).Get();
            var solicitud = response.Models.FirstOrDefault();
            if (solicitud is null)
            {
                return BadRequest("Hubo un error");
            }
            TipoSolicitudResponse tipo_solicitud;
            HttpResponseMessage tipoSolicitud_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/tipo-solicitud/id:int?id=" + solicitud.id_tipo_solicitud);
            string tipoSolicitudcontenido = await tipoSolicitud_res.Content.ReadAsStringAsync();
            tipo_solicitud = JsonConvert.DeserializeObject<TipoSolicitudResponse>(tipoSolicitudcontenido);
            if (tipo_solicitud == null)
            {
                return BadRequest("Hubo un error al recibir el tipo de solicitud");
            }

            EstatusResponse estatus;
            HttpResponseMessage estatus_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/estatus-solicitud/id:int?id=" + solicitud.id_estatus_solicitud);
            string estatus_contenido = await estatus_res.Content.ReadAsStringAsync();
            estatus = JsonConvert.DeserializeObject<EstatusResponse>(estatus_contenido);
            if (estatus == null)
            {
                return BadRequest("Hubo un error al recibir el estatus");
            }

            UsuarioResponse usuario;
            HttpResponseMessage usuario_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/usuario/id:int?id=" + solicitud.id_usuario);
            string usuario_contenido = await usuario_res.Content.ReadAsStringAsync();
            usuario = JsonConvert.DeserializeObject<UsuarioResponse>(usuario_contenido);
            if (usuario == null)
            {
                return BadRequest("Hubo un error al recibir el usuario");
            }

            var folio = solicitud.folio;
            var tipo_solicitud_int = solicitud.id_tipo_solicitud;

            LabResponse lab = new LabResponse { };
            List<EquipoResponse> equipoResponse = new List<EquipoResponse>();

            DateTime fecha_salida = new DateTime(2023, 5, 10);
            DateTime fecha_vuelta = new DateTime(2023, 5, 10);
            int personas = 0;

            if (solicitud.id_tipo_solicitud == 1) //equipos
            {
                var response_equipos = await _supabaseClient.From<Solicitud_Equipo>().Where(n => n.folio == id).Get();
                var solicitud_equipos = response_equipos.Models;
                if (solicitud_equipos is null)
                {
                    return BadRequest("Hubo un error");
                }
                fecha_salida = solicitud_equipos[0].fecha_salida;
                fecha_vuelta = solicitud_equipos[0].fecha_vuelta;
                foreach (Solicitud_Equipo sol_equipo in solicitud_equipos)
                {
                    EquipoResponse equipo;
                    HttpResponseMessage equipo_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/equipo/id:int?id=" + sol_equipo.equipo);
                    string equipo_contenido = await equipo_res.Content.ReadAsStringAsync();
                    equipo = JsonConvert.DeserializeObject<EquipoResponse>(equipo_contenido);
                    if (equipo == null)
                    {
                        return BadRequest("Hubo un error al recibir el usuario");
                    }
                    equipoResponse.Add(equipo);
                }
            }
            else if (solicitud.id_tipo_solicitud == 3) //labs
            {
                var response_lab = await _supabaseClient.From<Solicitud_Lab>().Where(n => n.folio == id).Get();
                var solicitud_lab = response_lab.Models.FirstOrDefault();
                if (solicitud_lab is null)
                {
                    return BadRequest("Hubo un error la recibir el laboratorio");
                }
                HttpResponseMessage lab_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/lab/id:int?id=" + solicitud_lab.laboratorio);
                string lab_contenido = await lab_res.Content.ReadAsStringAsync();
                lab = JsonConvert.DeserializeObject<LabResponse>(lab_contenido);
                if (lab == null)
                {
                    return BadRequest("Hubo un error al recibir el usuario");
                }
                fecha_salida = solicitud_lab.fecha_salida;
                fecha_vuelta = solicitud_lab.fecha_vuelta;
                personas = solicitud_lab.cantidad_personas;
            }

            var solicitudResponse = new SolicitudResponse
            {
                id = folio,
                fecha_pedido = solicitud.fecha_pedido,
                fecha_salida = fecha_salida,
                fecha_vuelta = fecha_vuelta,
                comentario = solicitud.comentario,
                tipo_solicitud = tipo_solicitud,
                estatus = estatus,
                usuario = usuario,
                lab = lab,
                equipos = equipoResponse,
                cantidad_personas = personas
            };
            return Ok(solicitudResponse);
        }
    }
}
