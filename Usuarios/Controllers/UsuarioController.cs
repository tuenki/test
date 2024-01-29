using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Usuarios.Models;
using Usuarios.Models.Custom;

namespace Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuariosContext _context;

        public UsuarioController(UsuariosContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        [Route("GetUsers")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsers()
        {
            var allUsers = await _context.Usuarios.ToListAsync();
            return Ok(new GeneralResponse() { Msg = "ok", StatusCode = true, Data = allUsers.Select(usuario => new UsuarioResponse
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                ApellidoMaterno = usuario.ApellidoMaterno,
                ApellidoPaterno = usuario.ApellidoMaterno,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                Usuario = usuario.Usuario1

            }) 
            });
        }

        [Authorize]
        [HttpGet]
        [Route("GetUsers/{name}")]
        public async Task<ActionResult> GetUsersId(string name)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Nombre == name);
            
            if (usuario == null)
            {
                return NotFound(new GeneralResponse() { Msg="Usuario no encontrado", StatusCode=false});
            }
            var response = new UsuarioResponse()
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                ApellidoMaterno = usuario.ApellidoMaterno,
                ApellidoPaterno = usuario.ApellidoMaterno,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                Usuario = usuario.Usuario1
            };
            return Ok(new GeneralResponse() {Msg="Ok",StatusCode=true,Data=response });
        }

        [Authorize]
        [HttpPost]
        [Route("RegisterUserRole")]

        public async Task<ActionResult<Usuario>> RegisterUserRole(Usuario usuario) 
        {
            var isExist = _context.Usuarios.FirstOrDefault(user =>
            user.Usuario1 == usuario.Usuario1 &&
            user.Password == usuario.Password
            );
            if (isExist == null) 
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return Ok(new GeneralResponse() { Msg = "Usuario registrado correctamente", StatusCode = true });
            }
            return NotFound(new GeneralResponse() { Msg = "Usuario ya existe", StatusCode = false });
            
        }
    }
}
