using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Usuarios.Models;
using Usuarios.Models.Custom;
namespace Usuarios.Services
{
    public class AutorizacionService:IAutorizacionService
    {
        private readonly IConfiguration _configuration;
        private readonly UsuariosContext _context;
        public AutorizacionService(UsuariosContext usuariosContext, IConfiguration configuration) 
        { 
            _context = usuariosContext;
            _configuration = configuration;
        }

        public string GenerateToken(string id_usuario) 
        {
            var key = _configuration.GetValue<string>("JwtKey:key");
            var bytesKey = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, id_usuario));

            var credencialesToken = new SigningCredentials( new SymmetricSecurityKey(bytesKey), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = credencialesToken,

            };

            var tokendHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokendHandler.CreateToken(tokenDescriptor);

            string token = tokendHandler.WriteToken(tokenConfig);
            
            return token;
        }

        public async Task<AutorizacionResponse> DevolverToken(AutorizacionRequest autorizacion) 
        { 
            var usuario = _context.Usuarios.FirstOrDefault(usuario => 
            usuario.Usuario1 == autorizacion.Username &&
            usuario.Password == autorizacion.Password
            );

            if (usuario == null) 
            {

                return await Task.FromResult<AutorizacionResponse>(new AutorizacionResponse() {Msg= "Sus datos son incorrectos, verifique su información e intente nuevamente", StatusCode=false,Token=null});
            }

            var token = GenerateToken(usuario.Id.ToString());
            return new AutorizacionResponse() {
                Token = token,
                StatusCode=true,
                Msg="Ok"
            };
        }
    }
}
