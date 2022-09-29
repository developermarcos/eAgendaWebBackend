using AutoMapper;
using eAgenda.Aplicacao.ModuloAutenticacao;
using eAgenda.Dominio.ModuloAutenticacao;
using eAgenda.Webapi.ViewModels.Autenticacao;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/conta")]
    [ApiController]
    public class AutenticacaoController : eAgendaControllerBase
    {
        private readonly ServicoAutenticacao servicoAutenticacao;
        private readonly IMapper mapeadorUsuario;

        public AutenticacaoController(ServicoAutenticacao servicoAutenticacao, IMapper mapeadorUsuario)
        {
            this.servicoAutenticacao=servicoAutenticacao;
            this.mapeadorUsuario=mapeadorUsuario;
        }
        [HttpPost("registrar")]
        public async Task<ActionResult> RegistrarUsuario(RegistrarUsuarioViewModel usuarioVM)
        {
            var usuario = mapeadorUsuario.Map<Usuario>(usuarioVM);

            var registrarUsuarioResult = await servicoAutenticacao.Registrarusuario(usuario, usuarioVM.Senha);

            if (registrarUsuarioResult.IsFailed)
                return InternalError(registrarUsuarioResult);

            return Ok(new
            {
                sucesso = true,
                dados = registrarUsuarioResult.Value
            });
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> AutenticarUsuario(AutenticarUsuarioViewModel usuarioVM)
        {
            
            var registrarUsuarioResult = await servicoAutenticacao.AutenticarUsuario(usuarioVM.Email, usuarioVM.Senha);

            if (registrarUsuarioResult.IsFailed)
                return BadRequest(registrarUsuarioResult);

            return Ok(new
            {
                sucesso = true,
                dados = GerarJst(registrarUsuarioResult.Value)
            });
        }
        private TokenViewModel GerarJst(Usuario usuario)
        {
            var identityClaims = new ClaimsIdentity();

            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, usuario.Email));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.GivenName, usuario.Nome));

            var tokerHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("segredoSuperSecretoDoeAgenda");

            var token = tokerHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "eAgenda",
                Audience = "http://localhost",
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokerHandler.WriteToken(token);

            var response = new TokenViewModel
            {
                Chave = encodedToken,
                UsuarioToken = new UsuarioTokenViewModel
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email
                }
            };

            return response;
        }
    }
}
