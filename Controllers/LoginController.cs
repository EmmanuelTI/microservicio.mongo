using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using uttt.edu.micro.loggin.aplicacion;
using uttt.edu.micro.loggin.modelo;

namespace uttt.edu.micro.loggin.api.Controllers
{
    [Route("api")]
    [ApiController] 
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("registro")]
        public async Task<ActionResult<Unit>> RegistrarUsuario([FromBody] Nuevo.Ejecuta data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _mediator.Send(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("usuarios")]
        public async Task<ActionResult<List<UsuarioDto>>> GetUsuarios()
        {
            var lista = await _mediator.Send(new Consulta.ListaUsuarios());
            return Ok(lista);
        }

        [HttpGet("usuario/{nombreUsuario}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuarioPorNombre(string nombreUsuario)
        {
            var usuario = await _mediator.Send(new Consulta.UsuarioPorNombre { NombreUsuario = nombreUsuario });
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> IniciarSesion([FromBody] Login.IniciarSesion request)
        {
            try
            {
                var usuario = await _mediator.Send(request);
                return Ok(usuario); 
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }
        [HttpPut("actualizarpassword")]
        public async Task<IActionResult> ActualizarPassword([FromBody] ActualizarPassword.EjecutaActualizarPassword comando)
        {
            var resultado = await _mediator.Send(comando);

            if (resultado)
                return Ok(new { mensaje = "Contraseña actualizada correctamente." });

            return BadRequest(new { mensaje = "No se pudo actualizar la contraseña." });
        }

    }
}
