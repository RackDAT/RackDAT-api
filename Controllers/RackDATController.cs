using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RackDAT_API.Contracts;
using RackDAT_API.Models;
namespace RackDAT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RackDATController : ControllerBase
    {

        private readonly Supabase.Client _supabaseClient;
        private readonly HttpClient _httpClient;
        public RackDATController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
            _httpClient = new HttpClient();
        }
        //-----------------------Tipo Usuario Endpoints---------------------------------------------------//
        [HttpGet("tipo-usuario/id:int")]
        public async Task<IActionResult> getTipoUsuarioID(int id)
        {
            var response = await _supabaseClient.From<Tipo_Usuario>().Where(n => n.id == id).Get();
            var tipo_usuario = response.Models.FirstOrDefault();
            if (tipo_usuario is null)
            {
                return NotFound("tipo de usuario no encontrada");
            }
            var tipoUsuarioResponse = new TipoUsuarioResponse
            {
                id = tipo_usuario.id,
                tipo_usuario = tipo_usuario.tipo_usuario
            };
            return Ok(tipoUsuarioResponse);
        }

        [HttpGet("usuario/id:int/solicitudes")] //todas las solicitudes
        public async Task<ActionResult> getSolicitudesUsuario(int id)
        {
            var response = await _supabaseClient.From<Solicitud>().Where(n => n.id_usuario == id).Order(n => n.fecha_pedido, Postgrest.Constants.Ordering.Descending).Get();
            var sol_equipoContenido = response.Models;
            if (sol_equipoContenido is null)
            {

                return NotFound("No hay solicitudes de equipos por desplegar");
            }

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


        //-----------------------Salones Endpoints-------------------------------------------------------//
        [HttpPost("salon")]
        public async Task<IActionResult> postSalon(CreateSalonRequest request)
        {
            var salon = new Salon
            {
                salon = request.salon,
                descripcion = request.descripcion,
            };

            var response = await _supabaseClient.From<Salon>().Insert(salon);

            var newSalon = response.Models.First();

            var salonResponse = new SalonResponse
            {
                id = newSalon.id,
                salon = newSalon.salon,
                descripcion = newSalon.descripcion,

            };

            return Ok(salonResponse);
        }

        [HttpGet("salon/id:int")]
        public async Task<IActionResult> getSalonID(int id)
        {
            var response = await _supabaseClient.From<Salon>().Where(n => n.id == id).Get();
            var salon = response.Models.FirstOrDefault();
            if (salon is null)
            {
                return NotFound("Salon no encontrado");
            }
            var salonResponse = new SalonResponse
            {
                id = salon.id,
                salon = salon.salon,
                descripcion = salon.descripcion
            };
            return Ok(salonResponse);
        }

        [HttpGet("salones")]
        public async Task<ActionResult<IEnumerable<SalonResponse>>> getSalon()
        {
            var response = await _supabaseClient.From<Salon>().Get();
            var getResponse = response.Models;
            if (getResponse is null)
            {
                return NotFound();
            }
            List<SalonResponse> salonResponse = new List<SalonResponse>();
            foreach (Salon salon in getResponse)
            {
                salonResponse.Add(new SalonResponse
                {
                    id = salon.id,
                    salon = salon.descripcion,
                    descripcion = salon.descripcion,
                }
                );
            }
            return Ok(salonResponse);
        }
        
        //-----------------------Tipo Solicitudes Endpoints-------------------------------------------------------//
        [HttpGet("tipo-solicitud/id:int")]
        public async Task<IActionResult> getTipoSolicitud(int id)
        {
            var response = await _supabaseClient.From<Tipo_Solicitud>().Where(n => n.id == id).Get();
            var tipo_solicitud = response.Models.FirstOrDefault();
            if (tipo_solicitud is null)
            {
                return NotFound("Tipo de solicitud no encontrado");
            }
            var tipo_SolicitudResponse = new TipoSolicitudResponse
            {
                id = tipo_solicitud.id,
                tipo_solicitud = tipo_solicitud.tipo_solicitud
            };
            return Ok(tipo_SolicitudResponse);
        }

        //-----------------------Estatus Solicitudes Endpoints-------------------------------------------------------//
        [HttpGet("estatus-solicitud/id:int")]
        public async Task<IActionResult> getEstatusSolicitud(int id)
        {
            var response = await _supabaseClient.From<Estatus_Solicitud>().Where(n => n.id == id).Get();
            var estatus_solicitud = response.Models.FirstOrDefault();
            if (estatus_solicitud is null)
            {
                return NotFound("Estatus de solicitud no encontrado");
            }
            var estatus_solicitudResponse = new EstatusResponse
            {
                id = estatus_solicitud.id,
                estatus = estatus_solicitud.estatus_solicitud
            };
            return Ok(estatus_solicitudResponse);
        }

        


        //---------------------------------------Solicitud Lab-------------------------------------------------// Porque estoy bien wey y este era el de equipos
        [HttpPost("solicitud/lab")]
        public async Task<IActionResult> postSolicitudLab(CreateSolicitudLabRequest request)
        {
            var solicitud_lab = new Solicitud_Lab
            {
                folio = request.folio,
                laboratorio = request.lab,
                fecha_salida = request.inicio,
                fecha_vuelta = request.final,
                cantidad_personas = request.cantidad_personas
            };
            var response = await _supabaseClient.From<Solicitud_Lab>().Insert(solicitud_lab);
            var sol_lab = response.Models.FirstOrDefault();
            if (sol_lab is null)
            {
                return NotFound("hubo un error al crear la solicitud de laboratorio");
            }
            HttpResponseMessage lab_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/lab/id:int?id=" + sol_lab.laboratorio);
            string lab_contenido = await lab_res.Content.ReadAsStringAsync();
            LabResponse lab = JsonConvert.DeserializeObject<LabResponse>(lab_contenido);
            if (lab == null)
            {
                return BadRequest("Hubo un error al recibir el laboratorio");
            }
            HttpResponseMessage solicitud_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/solicitud/id:int?id=" + sol_lab.folio);
            string solicitud_contenido = await solicitud_res.Content.ReadAsStringAsync();
            SolicitudResponse solicitud = JsonConvert.DeserializeObject<SolicitudResponse>(solicitud_contenido);
            if (solicitud == null)
            {
                return BadRequest("Hubo un error al recibir la solicitud");
            }

            var sol_labResponse = new SolicitudLabResponse
            {
                inicio = sol_lab.fecha_salida,
                final = sol_lab.fecha_vuelta,
                cantidad_personas = sol_lab.cantidad_personas,
                lab = lab,
                folio = solicitud
            };
                        
            return Ok(sol_labResponse);
        }

        //---------------------------------------Solicitud Lab-------------------------------------------------//
        [HttpGet("solicitudes-lab")]
        public async Task<IActionResult> getSolicitudesLab()
        {
            var response = await _supabaseClient.From<Solicitud>().Where(n => n.id_tipo_solicitud == 3 && n.id_estatus_solicitud == 3).Get();
            var sol_equipoContenido = response.Models;
            if (sol_equipoContenido is null)
            {

                return NotFound("No hay solicitudes de laboratorios por desplegar");
            }

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

                sol_equipoResponse.Add(new SolicitudResponse
                {
                    id = solicitud.folio,
                    fecha_pedido = solicitud.fecha_pedido,
                    comentario = solicitud.comentario,
                    tipo_solicitud = tipo_solicitud,
                    estatus = estatus,
                    usuario = usuario,
                }
                );
            }
            return Ok(sol_equipoResponse);
        }


    }


}
