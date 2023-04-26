using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RackDAT_API.Contracts;
using RackDAT_API.Models;
using Supabase;
using System.Collections;
using System.Net.Http;

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
        [HttpPost ("carrera")]
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
                HttpResponseMessage carrera_res = await _httpClient.GetAsync("https://localhost:7188/api/RackDAT/carrera/id:int?id=" + newUsuario.id_carrera);
                string carrera_contenido = await carrera_res.Content.ReadAsStringAsync();
                carrera = JsonConvert.DeserializeObject<CarreraResponse>(carrera_contenido);
                if(carrera == null)
                {
                    return BadRequest("Hubo un error");
                }
            }

            HttpResponseMessage tipo_usuario_res = await _httpClient.GetAsync("https://localhost:7188/api/RackDAT/tipo-usuario/id:int?id=" + newUsuario.id_tipo_usuario);
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
                carrera = carrera
            };


            return Ok(usuarioResponse);
        }

        //-----------------------Salones Endpoints-------------------------------------------------------//
        [HttpPost("salon")]
        public async Task<IActionResult> postSalon(CreateSalonRequest request)
        {
            var salon = new Salon
            {
                salon = request.salon,
                descripcion = request.descripcion,
                imagen = request.imagen
            };

            var response = await _supabaseClient.From<Salon>().Insert(salon);

            var newSalon = response.Models.First();

            var salonResponse = new SalonResponse
            {
                id = newSalon.id,
                salon = newSalon.salon,
                descripcion = newSalon.descripcion,
                imagen = newSalon.imagen,

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
                descripcion = salon.descripcion,
                imagen = salon.imagen
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
                    imagen = salon.imagen
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
                salon = request.salon
                
            };

            var response = await _supabaseClient.From<Laboratorio>().Insert(laboratorio);

            var newLab = response.Models.First();

            //Get salon
            SalonResponse salon = new SalonResponse { };
            HttpResponseMessage salon_res = await _httpClient.GetAsync("https://localhost:7188/api/RackDAT/salon/id:int?id=" + laboratorio.salon);
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
                salon = salon

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
                HttpResponseMessage salon_res = await _httpClient.GetAsync("https://localhost:7188/api/RackDAT/salon/id:int?id=" + lab.salon);
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
                    salon = salon
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
            HttpResponseMessage salon_res = await _httpClient.GetAsync("https://localhost:7188/api/RackDAT/salon/id:int?id=" + lab.salon);
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
                salon = salon
            };
            return Ok(labResponse);
        }

        //-------------------Pruebas-------------------------//
        

    }

}
