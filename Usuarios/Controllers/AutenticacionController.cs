using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Usuarios.Models;
using Usuarios.Models.Custom;
using Usuarios.Services;

namespace Usuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly IAutorizacionService _autorizacionService;

        public AutenticacionController(IAutorizacionService autorizacionService)
        {
            _autorizacionService = autorizacionService;
        }

        [HttpPost]
        [Route("authentication")]
        public async Task<ActionResult> Autenticar([FromBody] AutorizacionRequest autorizacion)
        {
            var resultado = await _autorizacionService.DevolverToken(autorizacion);
            if (resultado == null) 
            { 
                return Unauthorized();
            }
            return Ok(resultado);
        }
    }
}
