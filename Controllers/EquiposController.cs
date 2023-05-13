using Microsoft.AspNetCore.Mvc;
using RackDAT_API.Models;
using RackDAT_API.Contracts;
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

            return Ok(JsonConvert.SerializeObject(newEquipo));
        }

        [HttpGet("equipo/{id}")]
        public async Task<IActionResult> getEquipoID(int id)
        {
            var response = await _supabaseClient.From<Equipo>().Where(n => n.id == id).Get();
            var equipo = response.Models.FirstOrDefault();
            return Ok(JsonConvert.SerializeObject(equipo));
        }

        [HttpGet("equipos")]
        public async Task<IActionResult> getEquipos()
        {
            var response = await _supabaseClient.From<Equipo>().Get();
            var equipos = response.Models;
            
            return Ok(JsonConvert.SerializeObject(equipos));
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

        //-----------------------Proveedores-------------------------------------------------------//
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

            return Ok(JsonConvert.SerializeObject(newProveedor));
        }

        [HttpGet("proveedor/{id}")]
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
            var proveedores = response.Models;
            
            return Ok(JsonConvert.SerializeObject(proveedores));
        }
    }
}
