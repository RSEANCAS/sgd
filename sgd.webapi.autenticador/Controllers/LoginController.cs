using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using sgd.be;
using sgd.bl.Contrato;
using sgd.webapi.autenticador.Models;
using sgd.webapi.autenticador.Models.Request;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace sgd.webapi.autenticador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        IConfiguration _config;
        IUsuarioBl _usuarioBl;

        public LoginController(IConfiguration config, IUsuarioBl usuarioBl)
        {
            _config = config;
            _usuarioBl = usuarioBl;
        }

        [HttpPost]
        [Route("autenticar")]
        public IActionResult PostAutenticar([FromBody] LoginRequest pLoginRequest)
        {
            bool vEsValido = _usuarioBl.AutenticarUsuario(pLoginRequest.AplicacionCodigo, pLoginRequest.UsuarioId, pLoginRequest.Contraseña, out UsuarioBe? poUsuario, out List<string> poListaMensajeError);

            if (!vEsValido)
            {
                return Unauthorized(new { Success = vEsValido, ListaMensajeError = poListaMensajeError });
            }
            string? vSecurityKeyStr = _config["Jwt:Key"];
            string? vIssuerStr = _config["Jwt:Issuer"];

            List<string> vListaMensajeError = new List<string>();

            if (vSecurityKeyStr == null) vListaMensajeError.Add("No se encontró la ruta Jwt:Key");
            if (vIssuerStr == null) vListaMensajeError.Add("No se encontró la ruta Jwt:Issuer");

            vEsValido = vListaMensajeError.Count == 0;

            if (!vEsValido)
            {
                return Problem(detail: string.Join(Environment.NewLine, vListaMensajeError));
            }

            string vUsuarioStr = JsonConvert.SerializeObject(poUsuario);
            UsuarioResponse? vUsuarioResponse = JsonConvert.DeserializeObject<UsuarioResponse>(vUsuarioStr);
            string vUsuarioCodificadoStr = Convert.ToBase64String(Encoding.UTF8.GetBytes(vUsuarioStr));

            var vSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(vSecurityKeyStr));
            var vCredentials = new SigningCredentials(vSecurityKey, SecurityAlgorithms.HmacSha256);

            var vSectoken = new JwtSecurityToken(vIssuerStr, vIssuerStr, claims: new List<Claim> { new Claim ("usr", vUsuarioCodificadoStr) }, expires: DateTime.Now.AddMinutes(120), signingCredentials: vCredentials);

            var vToken = new JwtSecurityTokenHandler().WriteToken(vSectoken);

            return Ok(new { Success = vEsValido, Token = vToken });
        }
    }
}