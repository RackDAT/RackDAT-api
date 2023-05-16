using Microsoft.AspNetCore.Mvc;
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
                id_modelo = request.id_modelo,
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
                id_proveedor = request.proveedor
            };

            var response = await _supabaseClient.From<Modelo>().Insert(modelo);

            var newModelo = response.Models.First();

            return Ok(JsonConvert.SerializeObject(newModelo));
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
            return Ok(JsonConvert.SerializeObject(modelo));
        }

        [HttpGet("modelos")]
        public async Task<IActionResult> getModelos()
        {
            var response = await _supabaseClient.From<Modelo>().Get();
            var modelos = response.Models;
            if (modelos is null)
            {

                return NotFound("No hay modelos por desplegar");
            }

            return Ok(JsonConvert.SerializeObject(modelos));
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

            return Ok(JsonConvert.SerializeObject(proveedor));
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
