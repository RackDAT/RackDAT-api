﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RackDAT_API.Contracts;
using RackDAT_API.Models;
using Supabase;
using System.Collections;
using System.Net.Http;
using System.Reflection;

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
                carrera = carrera
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
                    }
                );
            }
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
                salon = salon
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

            var equipoResponse = new EquipoResponse
            {
                id = equipo.id,
                num_serie = equipo.ns,
                tag = equipo.tag,
                modelo = modelo,
                fecha_compra = equipo.fecha_compra,
                descripcion = equipo.descripcion,
                imagen = equipo.imagen,
                comentario = equipo.comentario
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

                equipoResponse.Add(new EquipoResponse
                {
                    id = equipo.id,
                    num_serie = equipo.ns,
                    tag = equipo.tag,
                    modelo = modelo,
                    fecha_compra = equipo.fecha_compra,
                    descripcion = equipo.descripcion,
                    imagen = equipo.imagen,
                    comentario = equipo.comentario
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


    }

}
