using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RackDAT_API.Contracts;
using RackDAT_API.Models;
using Supabase;
using System.Collections;
using System.Net.Http;
using System.Reflection;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using System.Net;

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

        //-----------------------Carreras Endpoints---------------------------------------------------//
        [HttpPost("carrera")]
        [Authorize]
        public async Task<IActionResult> postCarrera(CreateCarreraRequest request)
        {
            var carrera = new Carrera
            {
                carrera = request.carrera,
                siglas = request.siglas
            };

            var response = await _supabaseClient.From<Carrera>().Insert(carrera);

            var newCarrera = response.Models.First();

            var carreraResponse = new CarreraResponse
            {
                id = newCarrera.id,
                carrera = newCarrera.carrera,
                siglas = newCarrera.siglas
            };

            return Ok(carreraResponse);
        }


        [HttpGet("carrera/id:int")]
        public async Task<IActionResult> getCarreraID(int id)
        {
            var response = await _supabaseClient.From<Carrera>().Where(n=>n.id == id).Get();
            var carrera = response.Models.FirstOrDefault();
            if(carrera is null)
            {
                return NotFound("Carrera no encontrada");
            }
            var carreraResponse = new CarreraResponse
            {
                id = carrera.id,
                carrera = carrera.carrera,
                siglas = carrera.siglas
            };
            return Ok(carreraResponse);
        }

        [HttpGet("carreras")]
        public async Task<ActionResult<IEnumerable<CarreraResponse>>> getCarrera()
        {
            var response = await _supabaseClient.From<Carrera>().Get();
            var carrerasR = response.Models;
            if (carrerasR is null)
            {
                return NotFound();
            }
            List<CarreraResponse> regresar = new List<CarreraResponse>();
            foreach(Carrera carrera in carrerasR)
            {
                regresar.Add(new CarreraResponse 
                    { 
                        id = carrera.id,
                        carrera = carrera.carrera,
                        siglas = carrera.siglas
                    }
                );
            }
            return Ok(regresar);
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


        //-----------------------Usuario Endpoints-------------------------------------------------------//
        [HttpPost("usuario")]
        public async Task<IActionResult> postUsuario(CreateUsuarioRequest request)
        {
            var usuario = new Usuario
            {
                nombre = request.nombre,
                apellido_pat = request.apellido_pat,
                apellido_mat = request.apellido_mat,
                correo = request.correo,
                clave = request.clave,
                id_tipo_usuario = request.tipo_usuario,
                id_carrera = request.carrera,
                imagen = request.imagen
            };

            //Comprobacion que el correo y la clave no esten en uso
            var correoGet = await _supabaseClient.From<Usuario>().Where(n => n.correo == usuario.correo).Get();
            var correoRes = correoGet.Models.FirstOrDefault();
            if (correoRes != null)
            {
                return BadRequest("El correo ya esta vinculado con una cuenta");
            }

            var claveGet = await _supabaseClient.From<Usuario>().Where(n => n.clave == usuario.clave).Get();
            var claveRes = claveGet.Models.FirstOrDefault();
            if (claveRes != null)
            {
                return BadRequest("La matricula ya esta vinculado a una cuenta");
            }

            var response = await _supabaseClient.From<Usuario>().Insert(usuario);

            var newUsuario = response.Models.First();


            if(newUsuario == null)
            {
                return BadRequest("Hubo un error al crear el usuario");
            }

            CarreraResponse carrera = new CarreraResponse { };
            TipoUsuarioResponse tipo_usuario = new TipoUsuarioResponse { };
            if (newUsuario.id_carrera != null)
            {
                //referencia otro endpoint
                HttpResponseMessage carrera_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/carrera/id:int?id=" + newUsuario.id_carrera);
                string carrera_contenido = await carrera_res.Content.ReadAsStringAsync();
                carrera = JsonConvert.DeserializeObject<CarreraResponse>(carrera_contenido);
                if(carrera == null)
                {
                    return BadRequest("Hubo un error");
                }
            }

            HttpResponseMessage tipo_usuario_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/tipo-usuario/id:int?id=" + newUsuario.id_tipo_usuario);
            string tipo_usuario_contenido = await tipo_usuario_res.Content.ReadAsStringAsync();
            tipo_usuario = JsonConvert.DeserializeObject<TipoUsuarioResponse>(tipo_usuario_contenido);
            if(tipo_usuario == null)
            {
                return BadRequest("Hubo un error");
            }

            var usuarioResponse = new UsuarioResponse
            {
                id = newUsuario.id,
                nombre = newUsuario.nombre,
                apellido_pat = newUsuario.apellido_pat,
                apellido_mat = newUsuario.apellido_mat,
                correo = newUsuario.correo,
                clave = newUsuario.clave,
                tipo_usuario = tipo_usuario,
                carrera = carrera,
                verificado = newUsuario.verificado,
                imagen = newUsuario.imagen
            };


            return Ok(usuarioResponse);
        }

        [HttpGet("usuario/id:int")]
        public async Task<ActionResult> getUsuarioID(int id)
        {
            var response = await _supabaseClient.From<Usuario>().Where(n => n.id == id).Get();
            var usuario = response.Models.FirstOrDefault();
            if (usuario is null)
            {
                return BadRequest("Hubo un error");
            }

            CarreraResponse carrera = new CarreraResponse { };
            TipoUsuarioResponse tipo_usuario = new TipoUsuarioResponse { };

            //referencia para sacar la carrera
            HttpResponseMessage carrera_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/carrera/id:int?id=" + usuario.id_carrera);
            string carrera_contenido = await carrera_res.Content.ReadAsStringAsync();
            carrera = JsonConvert.DeserializeObject<CarreraResponse>(carrera_contenido);
            if (carrera == null)
            {
                return BadRequest("Hubo un error");
            }

            //referencia para sacar el tipo de usuario
            HttpResponseMessage tipo_usuario_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/tipo-usuario/id:int?id=" + usuario.id_tipo_usuario);
            string tipo_usuario_contenido = await tipo_usuario_res.Content.ReadAsStringAsync();
            tipo_usuario = JsonConvert.DeserializeObject<TipoUsuarioResponse>(tipo_usuario_contenido);
            if (tipo_usuario == null)
            {
                return BadRequest("Hubo un error");
            }

            var usuarioResponse = new UsuarioResponse
            {
                id = usuario.id,
                nombre = usuario.nombre,
                apellido_pat = usuario.apellido_pat,
                apellido_mat = usuario.apellido_mat,
                correo = usuario.correo,
                clave = usuario.clave,
                tipo_usuario = tipo_usuario,
                carrera = carrera,
                verificado = usuario.verificado,
                imagen = usuario.imagen

            };
            return Ok(usuarioResponse);
        }

        [HttpPost("usuario/correo")] //usuario por correo
        public async Task<ActionResult> getUsuarioCorreo(CorreoRequest request)
        {
            var response = await _supabaseClient.From<Usuario>().Where(n => n.correo == request.correo).Get();
            var usuario = response.Models.FirstOrDefault();
            if (usuario is null)
            {
                return NoContent();
            }

            CarreraResponse carrera = new CarreraResponse { };
            TipoUsuarioResponse tipo_usuario = new TipoUsuarioResponse { };

            //referencia para sacar la carrera
            HttpResponseMessage carrera_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/carrera/id:int?id=" + usuario.id_carrera);
            string carrera_contenido = await carrera_res.Content.ReadAsStringAsync();
            carrera = JsonConvert.DeserializeObject<CarreraResponse>(carrera_contenido);
            if (carrera == null)
            {
                return BadRequest("Hubo un error");
            }

            //referencia para sacar el tipo de usuario
            HttpResponseMessage tipo_usuario_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/tipo-usuario/id:int?id=" + usuario.id_tipo_usuario);
            string tipo_usuario_contenido = await tipo_usuario_res.Content.ReadAsStringAsync();
            tipo_usuario = JsonConvert.DeserializeObject<TipoUsuarioResponse>(tipo_usuario_contenido);
            if (tipo_usuario == null)
            {
                return BadRequest("Hubo un error");
            }

            var usuarioResponse = new UsuarioResponse
            {
                id = usuario.id,
                nombre = usuario.nombre,
                apellido_pat = usuario.apellido_pat,
                apellido_mat = usuario.apellido_mat,
                correo = usuario.correo,
                clave = usuario.clave,
                tipo_usuario = tipo_usuario,
                carrera = carrera,
                verificado = usuario.verificado,
                imagen = usuario.imagen

            };
            return Ok(usuarioResponse);
        }

        [HttpGet("usuarios")]
        public async Task<ActionResult> getUsuarios()
        {
            var response = await _supabaseClient.From<Usuario>().Get();
            var usuario_contenido = response.Models;
            if (usuario_contenido is null)
            {
                return NotFound("No hay usuarios por desplegar");
            }

            List<UsuarioResponse> usuarioResponse = new List<UsuarioResponse>();
            foreach (Usuario usuario in usuario_contenido)
            {

                CarreraResponse carrera = new CarreraResponse { };
                TipoUsuarioResponse tipo_usuario = new TipoUsuarioResponse { };

                //referencia para sacar la carrera
                HttpResponseMessage carrera_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/carrera/id:int?id=" + usuario.id_carrera);
                string carrera_contenido = await carrera_res.Content.ReadAsStringAsync();
                carrera = JsonConvert.DeserializeObject<CarreraResponse>(carrera_contenido);
                if (carrera == null)
                {
                    return BadRequest("Hubo un error");
                }

                //referencia para sacar el tipo de usuario
                HttpResponseMessage tipo_usuario_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/tipo-usuario/id:int?id=" + usuario.id_tipo_usuario);
                string tipo_usuario_contenido = await tipo_usuario_res.Content.ReadAsStringAsync();
                tipo_usuario = JsonConvert.DeserializeObject<TipoUsuarioResponse>(tipo_usuario_contenido);
                if (tipo_usuario == null)
                {
                    return BadRequest("Hubo un error");
                }

                usuarioResponse.Add(new UsuarioResponse
                {
                    id = usuario.id,
                    nombre = usuario.nombre,
                    apellido_pat = usuario.apellido_pat,
                    apellido_mat = usuario.apellido_mat,
                    correo = usuario.correo,
                    clave = usuario.clave,
                    tipo_usuario = tipo_usuario,
                    carrera = carrera,
                    verificado = usuario.verificado,
                    imagen = usuario.imagen
                }
                );
            }
            return Ok(usuarioResponse);
        }

        [HttpPut("usuario/id:int")]
        public async Task<ActionResult> verificarUsuario(int id, bool verificacion)
        {
            var update = await _supabaseClient.From<Usuario>().Where(n => n.id == id).Set(x => x.verificado, verificacion).Update();

            var response = await _supabaseClient.From<Usuario>().Where(n => n.id == id).Get();
            var usuario = response.Models.FirstOrDefault();
            if (usuario is null)
            {
                return BadRequest("Hubo un error");
            }

            CarreraResponse carrera = new CarreraResponse { };
            TipoUsuarioResponse tipo_usuario = new TipoUsuarioResponse { };

            HttpResponseMessage carrera_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/carrera/id:int?id=" + usuario.id_carrera);
            string carrera_contenido = await carrera_res.Content.ReadAsStringAsync();
            carrera = JsonConvert.DeserializeObject<CarreraResponse>(carrera_contenido);
            if (carrera == null)
            {
                return BadRequest("Hubo un error");
            }

            //referencia para sacar el tipo de usuario
            HttpResponseMessage tipo_usuario_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/tipo-usuario/id:int?id=" + usuario.id_tipo_usuario);
            string tipo_usuario_contenido = await tipo_usuario_res.Content.ReadAsStringAsync();
            tipo_usuario = JsonConvert.DeserializeObject<TipoUsuarioResponse>(tipo_usuario_contenido);
            if (tipo_usuario == null)
            {
                return BadRequest("Hubo un error");
            }

            var usuarioResponse = new UsuarioResponse
            {
                id = usuario.id,
                nombre = usuario.nombre,
                apellido_pat = usuario.apellido_pat,
                apellido_mat = usuario.apellido_mat,
                correo = usuario.correo,
                clave = usuario.clave,
                tipo_usuario = tipo_usuario,
                carrera = carrera,
                verificado = usuario.verificado,
                imagen = usuario.imagen

            };
            return Ok(usuarioResponse);
        }

        [HttpDelete("usuario/id:int")]
        public async Task<ActionResult> deleteUsuario(int id)
        {
            var response = await _supabaseClient.From<Usuario>().Where(n => n.id == id).Get();
            var usuario = response.Models.FirstOrDefault();
            if (usuario is null)
            {
                return BadRequest("Hubo un error");
            }
            if (usuario.verificado == true)
            {
                return BadRequest("No se puede borrar un usuario verificado");
            }
            await _supabaseClient.From<Usuario>().Where(n => n.id == id && n.verificado == false).Delete();
            return NoContent();
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


        //-----------------------Labs Endpoints-------------------------------------------------------//
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

            //Get salon
            SalonResponse salon = new SalonResponse { };
            HttpResponseMessage salon_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/salon/id:int?id=" + laboratorio.salon);
            string salon_contenido = await salon_res.Content.ReadAsStringAsync();
            salon = JsonConvert.DeserializeObject<SalonResponse>(salon_contenido);
            if (salon == null)
            {
                return BadRequest("Hubo un error");
            }
            
            var labResponse = new LabResponse
            {
                id = newLab.id,
                lab = newLab.laboratorio,
                salon = salon,
                imagen = newLab.imagen,
                descripcion = newLab.descripcion_lab
            };

            return Ok(labResponse);
        }
        [HttpGet("labs")]
        public async Task<ActionResult<IEnumerable<LabResponse>>> getLab()
        {
            var response = await _supabaseClient.From<Laboratorio>().Get();
            var lab_contenido = response.Models;
            if (lab_contenido is null)
            {
                return NotFound("No hay laboratorios por desplegar");
            }
            List<LabResponse> labResponse = new List<LabResponse>();
            foreach (Laboratorio lab in lab_contenido)
            {
                SalonResponse salon = new SalonResponse { };
                HttpResponseMessage salon_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/salon/id:int?id=" + lab.salon);
                string salon_contenido = await salon_res.Content.ReadAsStringAsync();
                salon = JsonConvert.DeserializeObject<SalonResponse>(salon_contenido);
                if (salon == null)
                {
                    return BadRequest("Hubo un error");
                }

                labResponse.Add(new LabResponse
                {
                    id = lab.id,
                    lab = lab.laboratorio,
                    salon = salon,
                    imagen = lab.imagen,
                    descripcion = lab.descripcion_lab
                }
                );
            }
            return Ok(labResponse);
        }

        [HttpGet("lab/id:int")]
        public async Task<ActionResult<LabResponse>> getLabID(int id)
        {
            var response = await _supabaseClient.From<Laboratorio>().Where(n => n.id == id).Get();
            var lab = response.Models.FirstOrDefault();
            if (lab is null)
            {
                return NotFound("laboratorio no encontrada");
            }

            SalonResponse salon = new SalonResponse { };
            HttpResponseMessage salon_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/salon/id:int?id=" + lab.salon);
            string salon_contenido = await salon_res.Content.ReadAsStringAsync();
            salon = JsonConvert.DeserializeObject<SalonResponse>(salon_contenido);
            if (salon == null)
            {
                return BadRequest("Hubo un error");
            }

            var labResponse = new LabResponse
            {
                id = lab.id,
                lab = lab.laboratorio,
                salon = salon,
                imagen = lab.imagen,
                descripcion = lab.descripcion_lab
            };
            return Ok(labResponse);
        }

        //-----------------------Equipo Endpoints-------------------------------------------------------//

        [HttpPost("equipo")]
        public async Task<IActionResult> postEquipo(CreateEquipoRequest request)
        {
                       

            var equipo = new Equipo
            {
                ns = request.num_serie,
                descripcion = request.descripcion,
                fecha_compra = request.fecha_compra,
                tag = request.tag,
                modelo = request.modelo,
                imagen = request.imagen,
                comentario = request.comentario,
                id_estanteria = request.estanteria
            };

            var response = await _supabaseClient.From<Equipo>().Insert(equipo);

            var newEquipo = response.Models.First();

            ModeloResponse modelo;
            HttpResponseMessage modelo_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/modelo/id:int?id=" + equipo.modelo);
            string modelo_contenido = await modelo_res.Content.ReadAsStringAsync();
            modelo = JsonConvert.DeserializeObject<ModeloResponse>(modelo_contenido);
            if(modelo is null)
            {
                return BadRequest("Hubo un error al obtener el modelo");
            }

            EstanteriaResponse estanteria;
            HttpResponseMessage estanteria_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/estanteria/id:int?id=" + equipo.id_estanteria);
            string estanteria_contenido = await estanteria_res.Content.ReadAsStringAsync();
            estanteria = JsonConvert.DeserializeObject<EstanteriaResponse>(estanteria_contenido);
            if (estanteria == null)
            {
                return BadRequest("Hubo un error al obtener la estanteria");
            }

            var equipoResponse = new EquipoResponse
            {
                id = newEquipo.id,
                modelo = modelo,
                num_serie = newEquipo.ns,
                descripcion = newEquipo.descripcion,
                tag = newEquipo.tag,
                imagen = newEquipo.imagen,
                fecha_compra = newEquipo.fecha_compra,
                comentario = newEquipo.comentario,
                estanteria = estanteria
            };

            return Ok(equipoResponse);
        }
        [HttpGet("equipo/id:int")]
        public async Task<IActionResult> getEquipoID(int id)
        {
            var response = await _supabaseClient.From<Equipo>().Where(n => n.id == id).Get();
            var equipo = response.Models.FirstOrDefault();
            if (equipo is null)
            {
                return NotFound("Equipo no encontrado");
            }
            ModeloResponse modelo;
            HttpResponseMessage modelo_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/modelo/id:int?id=" + equipo.modelo);
            string modelo_contenido = await modelo_res.Content.ReadAsStringAsync();
            modelo = JsonConvert.DeserializeObject<ModeloResponse>(modelo_contenido);
            if (modelo == null)
            {
                return BadRequest(modelo_res);
            }

            EstanteriaResponse estanteria;
            HttpResponseMessage estanteria_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/estanteria/id:int?id=" + equipo.id_estanteria);
            string estanteria_contenido = await estanteria_res.Content.ReadAsStringAsync();
            estanteria = JsonConvert.DeserializeObject<EstanteriaResponse>(estanteria_contenido);
            if (estanteria == null)
            {
                return BadRequest("Hubo un error al obtener la estanteria");
            }

            var equipoResponse = new EquipoResponse
            {
                id = equipo.id,
                num_serie = equipo.ns,
                tag = equipo.tag,
                modelo = modelo,
                fecha_compra = equipo.fecha_compra,
                descripcion = equipo.descripcion,
                imagen = equipo.imagen,
                comentario = equipo.comentario,
                estanteria = estanteria,
            };
            return Ok(equipoResponse);
        }

        [HttpGet("equipos")]
        public async Task<IActionResult> getEquipos()
        {
            var response = await _supabaseClient.From<Equipo>().Get();
            var equipoContenido = response.Models;
            if (equipoContenido is null)
            {

                return NotFound("No hay equipos por desplegar");
            }

            List<EquipoResponse> equipoResponse = new List<EquipoResponse>();
            foreach (Equipo equipo in equipoContenido)
            {
                ModeloResponse modelo;
                HttpResponseMessage modelo_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/modelo/id:int?id=" + equipo.modelo);
                string modelo_contenido = await modelo_res.Content.ReadAsStringAsync();
                modelo = JsonConvert.DeserializeObject<ModeloResponse>(modelo_contenido);
                if (modelo == null)
                {
                    return BadRequest("Hubo un error al obtener el modelo");
                }

                EstanteriaResponse estanteria;
                HttpResponseMessage estanteria_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/estanteria/id:int?id=" + equipo.id_estanteria);
                string estanteria_contenido = await estanteria_res.Content.ReadAsStringAsync();
                estanteria = JsonConvert.DeserializeObject<EstanteriaResponse>(estanteria_contenido);
                if (estanteria == null)
                {
                    return BadRequest("Hubo un error al obtener la estanteria");
                }

                equipoResponse.Add(new EquipoResponse
                {
                    id = equipo.id,
                    num_serie = equipo.ns,
                    tag = equipo.tag,
                    modelo = modelo,
                    fecha_compra = equipo.fecha_compra,
                    descripcion = equipo.descripcion,
                    imagen = equipo.imagen,
                    comentario = equipo.comentario,
                    estanteria = estanteria
                }
                );
            }
            return Ok(equipoResponse);
        }

        //-----------------------Modelo Endpoints-------------------------------------------------------//
        [HttpPost("modelo")]
        public async Task<IActionResult> postModelo(CreateModeloRequest request)
        {
            var comprobacion = await _supabaseClient.From<Modelo>().Where(n => n.modelo == request.modelo).Get();
            if (comprobacion != null)
            {
                return BadRequest("Ese modelo ya se encuentra registrado");
            }
            var modelo = new Modelo
            {
                modelo = request.modelo,
                proveedor = request.proveedor
            };

            var response = await _supabaseClient.From<Modelo>().Insert(modelo);

            var newModelo = response.Models.First();

            ProveedorResponse proveedor;
            HttpResponseMessage proveedor_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/proveedor/id:int?id=" + modelo.proveedor);
            string proveedor_contenido = await proveedor_res.Content.ReadAsStringAsync();
            proveedor = JsonConvert.DeserializeObject<ProveedorResponse>(proveedor_contenido);
            if (proveedor == null)
            {
                return BadRequest("Hubo un error");
            }

            var modeloResponse = new ModeloResponse
            {
                id = newModelo.id,
                modelo = newModelo.modelo,
                proveedor = proveedor
            };

            return Ok(modeloResponse);
        }

        [HttpGet("modelo/id:int")]
        public async Task<IActionResult> getModeloID(int id)
        {
            var response = await _supabaseClient.From<Modelo>().Where(n => n.id == id).Get();
            var modelo = response.Models.FirstOrDefault();
            if (modelo is null)
            {
                return NotFound("Modelo no encontrada");
            }
            ProveedorResponse proveedor;
            HttpResponseMessage proveedor_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/proveedor/id:int?id=" + modelo.proveedor);
            string proveedor_contenido = await proveedor_res.Content.ReadAsStringAsync();
            proveedor = JsonConvert.DeserializeObject<ProveedorResponse>(proveedor_contenido);
            if (proveedor == null)
            {
                return BadRequest("Hubo un error");
            }

            var modeloRespoonse = new ModeloResponse
            {
                id = modelo.id,
                modelo = modelo.modelo,
                proveedor = proveedor
            };
            return Ok(modeloRespoonse);
        }

        [HttpGet("modelos")]
        public async Task<IActionResult> getModelos()
        {
            var response = await _supabaseClient.From<Modelo>().Get();
            var modeloContenido = response.Models;
            if (modeloContenido is null)
            {

                return NotFound("No hay modelos por desplegar");
            }

            List<ModeloResponse> modeloResponse = new List<ModeloResponse>();
            foreach (Modelo modelo in modeloContenido)
            {
                ProveedorResponse proveedor;
                HttpResponseMessage proveedor_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/proveedor/id:int?id=" + modelo.proveedor);
                string proveedor_contenido = await proveedor_res.Content.ReadAsStringAsync();
                proveedor = JsonConvert.DeserializeObject<ProveedorResponse>(proveedor_contenido);
                if (proveedor == null)
                {
                    return BadRequest("Hubo un error");
                }

                modeloResponse.Add(new ModeloResponse
                {
                    id = modelo.id,
                    modelo = modelo.modelo,
                    proveedor = proveedor
                }
                );
            }
            return Ok(modeloResponse);
        }

        //-----------------------Proveedor Endpoints-------------------------------------------------------//
        [HttpPost("proveedor")]
        public async Task<IActionResult> postProveedor(CreateProveedorRequest request)
        {

            var comprobacion = await _supabaseClient.From<Proveedor>().Where(n => n.proveedor == request.proveedor).Get();
            if (comprobacion != null)
            {
                return BadRequest("Ese proveedor ya se encuentra registrado");
            }

            var proveedor = new Proveedor
            {
                proveedor = request.proveedor
            };

            var response = await _supabaseClient.From<Proveedor>().Insert(proveedor);

            var newProveedor = response.Models.First();

            var proveedorResponse = new ProveedorResponse
            {
                id = newProveedor.id,
                proveedor = newProveedor.proveedor
            };

            return Ok(proveedorResponse);
        }

        [HttpGet("proveedor/id:int")]
        public async Task<IActionResult> getProveedorID(int id)
        {
            var response = await _supabaseClient.From<Proveedor>().Where(n => n.id == id).Get();
            var proveedor = response.Models.FirstOrDefault();
            if (proveedor is null)
            {
                return NotFound("Proveedor no encontrado");
            }
            var proveedorResponse = new ProveedorResponse
            {
                id = proveedor.id,
                proveedor = proveedor.proveedor
            };
            return Ok(proveedorResponse);
        }

        [HttpGet("proveedores")]
        public async Task<IActionResult> getProveedor()
        {
            var response = await _supabaseClient.From<Proveedor>().Get();
            var proveedorContenido = response.Models;
            if (proveedorContenido is null)
            {

                return NotFound("No hay proveedores por desplegar");
            }

            List<ProveedorResponse> proveedorResponse = new List<ProveedorResponse>();
            foreach (Proveedor proveedor in proveedorContenido)
            {
                proveedorResponse.Add(new ProveedorResponse
                {
                    id = proveedor.id,
                    proveedor = proveedor.proveedor
                }
                );
            }
            return Ok(proveedorResponse);
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

            LabResponse lab;
            HttpResponseMessage lab_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/lab/id:int?id=" + newEstanteria.id_laboratorio);
            string lab_contenido = await lab_res.Content.ReadAsStringAsync();
            lab = JsonConvert.DeserializeObject<LabResponse>(lab_contenido);
            if (lab.descripcion == null)
            {
                return BadRequest("Hubo un error al recibir el laboratorio");
            }

            var estanteriaResponse = new EstanteriaResponse
            {
                id = newEstanteria.id,
                localidad = newEstanteria.estanteria,
                color = newEstanteria.color,
                lab = lab
            };

            return Ok(estanteriaResponse);
        }

        [HttpGet("estanteria/id:int")]
        public async Task<IActionResult> getEstanteriaID(int id)
        {
            var response = await _supabaseClient.From<Estanteria>().Where(n => n.id == id).Get();
            var estanteria = response.Models.FirstOrDefault();
            if (estanteria is null)
            {
                return NotFound("Estanteria no encontrada");
            }

            LabResponse lab;
            HttpResponseMessage lab_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/lab/id:int?id=" + estanteria.id_laboratorio);
            string lab_contenido = await lab_res.Content.ReadAsStringAsync();
            lab = JsonConvert.DeserializeObject<LabResponse>(lab_contenido);
            if (lab.descripcion == null)
            {
                return BadRequest("Hubo un error al recibir el laboratorio");
            }

            var estanteriaResponse = new EstanteriaResponse
            {
                id = estanteria.id,
                localidad = estanteria.estanteria,
                color = estanteria.color,
                lab = lab
            };
            return Ok(estanteriaResponse);
        }

        [HttpGet("estanterias")]
        public async Task<IActionResult> getEstanterias()
        {
            var response = await _supabaseClient.From<Estanteria>().Get();
            var estanteriaContenido = response.Models;
            if (estanteriaContenido is null)
            {

                return NotFound("No hay estanterias por desplegar");
            }

            List<EstanteriaResponse> estanteriaResponse = new List<EstanteriaResponse>();
            foreach (Estanteria estanteria in estanteriaContenido)
            {
                LabResponse lab;
                HttpResponseMessage lab_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/lab/id:int?id=" + estanteria.id_laboratorio);
                string lab_contenido = await lab_res.Content.ReadAsStringAsync();
                lab = JsonConvert.DeserializeObject<LabResponse>(lab_contenido);
                if (lab == null)
                {
                    return BadRequest("Hubo un error al recibir el laboratorio");
                }

                estanteriaResponse.Add(new EstanteriaResponse
                {
                    id = estanteria.id,
                    localidad = estanteria.estanteria,
                    color = estanteria.color,
                    lab = lab
                }
                );
            }
            return Ok(estanteriaResponse);
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

        //-----------------------Solicitudes Endpoints-------------------------------------------------------//

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

            TipoSolicitudResponse tipo_solicitud;
            HttpResponseMessage tipoSolicitud_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/tipo-solicitud/id:int?id=" + newSolicitud.id_tipo_solicitud);
            string tipoSolicitudcontenido = await tipoSolicitud_res.Content.ReadAsStringAsync();
            tipo_solicitud = JsonConvert.DeserializeObject<TipoSolicitudResponse>(tipoSolicitudcontenido);
            if (tipo_solicitud == null)
            {
                return BadRequest("Hubo un error al recibir el tipo de solicitud");
            }

            EstatusResponse estatus;
            HttpResponseMessage estatus_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/estatus-solicitud/id:int?id=" + newSolicitud.id_estatus_solicitud);
            string estatus_contenido = await estatus_res.Content.ReadAsStringAsync();
            estatus = JsonConvert.DeserializeObject<EstatusResponse>(estatus_contenido);
            if (estatus == null)
            {
                return BadRequest("Hubo un error al recibir el estatus");
            }

            UsuarioResponse usuario;
            HttpResponseMessage usuario_res = await _httpClient.GetAsync("https://rackdat.onrender.com/api/RackDAT/usuario/id:int?id=" + newSolicitud.id_usuario);
            string usuario_contenido = await usuario_res.Content.ReadAsStringAsync();
            usuario = JsonConvert.DeserializeObject<UsuarioResponse>(usuario_contenido);
            if (usuario == null)
            {
                return BadRequest("Hubo un error al recibir el usuario");
            }

            var solicitudResponse = new SolicitudResponse
            {
                id = newSolicitud.folio,
                fecha_pedido = newSolicitud.fecha_pedido,
                comentario = newSolicitud.comentario,
                tipo_solicitud = tipo_solicitud,
                estatus = estatus,
                usuario = usuario,
            };

            return Ok(solicitudResponse);
        }

        [HttpGet("solicitudes-pendientes")] //solicitudes pendientes de todas las personas
        public async Task<IActionResult> getSolicitudesPendientes()
        {
            var response = await _supabaseClient.From<Solicitud>().Where(n => n.id_estatus_solicitud == 3).Get();
            var sol_equipoContenido = response.Models;
            if (sol_equipoContenido is null)
            {

                return NotFound("No hay solicitudes pendientes por desplegar");
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
                else if(solicitud.id_tipo_solicitud == 3)
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
                    lab  = JsonConvert.DeserializeObject<LabResponse>(lab_contenido);
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

        [HttpPatch("solicitud-verificar/id:int")] //aceptar o rechazar solicitud
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
            else if(usuario.tipo_usuario.id == 4)
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
