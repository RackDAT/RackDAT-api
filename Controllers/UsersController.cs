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
    public class UsuariosController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly HttpClient _httpClient;

        public UsuariosController(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
            _httpClient = new HttpClient();
        }

        [HttpGet("GetUsuariosVerificados")]
        public async Task<IActionResult> getUsuariosVerificados()
        {
            var response = await _supabaseClient.From<Usuario>().Where(x => x.verificado == true).Get();

            return Ok(JsonConvert.SerializeObject(response.Models));
        }

        //---------------------------------------------------------------------------------------------------------//

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

            return Ok(JsonConvert.SerializeObject(newUsuario));
        }

        [HttpGet("usuario/{id}")]
        public async Task<ActionResult> getUsuarioID(int id)
        {
            var response = await _supabaseClient.From<Usuario>().Where(n => n.id == id).Get();
            var usuario = response.Models.FirstOrDefault();
            
            return Ok(JsonConvert.SerializeObject(usuario));
        }

        [HttpPost("usuario/correo")] //usuario por correo
        public async Task<ActionResult> getUsuarioCorreo(CorreoRequest request)
        {
            var response = await _supabaseClient.From<Usuario>().Where(n => n.correo == request.correo).Get();
            var usuario = response.Models.FirstOrDefault();
            
            return Ok(JsonConvert.SerializeObject(usuario));
        }

        [HttpGet("usuarios")]
        public async Task<ActionResult> getUsuarios()
        {
            var response = await _supabaseClient.From<Usuario>().Get();
            var usuarios = response.Models;
            
            return Ok(JsonConvert.SerializeObject(usuarios));
        }

        [HttpPut("usuario/{id}")]
        public async Task<ActionResult> verificarUsuario(int id, bool verificacion)
        {
            var update = await _supabaseClient.From<Usuario>().Where(n => n.id == id).Set(x => x.verificado, verificacion).Update();

            var response = await _supabaseClient.From<Usuario>().Where(n => n.id == id).Get();
            var usuario = response.Models.FirstOrDefault();

            return Ok(usuario);
        }

        [HttpDelete("usuario/{id}")]
        public async Task<ActionResult> deleteUsuario(int id)
        {
            await _supabaseClient.From<Usuario>().Where(n => n.id == id && n.verificado == false).Delete();
            return NoContent();
        }

        [HttpGet("solicitudes-historicas/{id}")] //solicitudes historicas de una persona
        public async Task<ActionResult> getSolicitudesHistoricas(int id)
        {
            var response = await _supabaseClient.From<Solicitud_Atributos>().Where(n => n.id_estatus_solicitud != 3 && n.id_usuario == id).Order(n => n.fecha_pedido, Postgrest.Constants.Ordering.Descending).Get();
            var solicitudes = response.Models;

            return Ok(JsonConvert.SerializeObject(solicitudes));

        }
        [HttpGet("usuarios/not-verificados")]
        public async Task<ActionResult> getUsuariosNoVerificados()
        {
            var response = await _supabaseClient.From<Usuario>().Where(n => n.verificado == false).Get();
            var usuarios = response.Models;
            return Ok(JsonConvert.SerializeObject(usuarios));
        }
        [HttpGet("usuarios/not-verificados/cantidad")]
        public async Task<ActionResult> getUsuariosNoVerificadosInt()
        {
            var response = await _supabaseClient.From<Usuario>().Where(n => n.verificado == false).Count(Postgrest.Constants.CountType.Exact);
            return Ok(response);
        }

    }
}
