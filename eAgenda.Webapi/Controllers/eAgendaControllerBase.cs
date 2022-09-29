using eAgenda.Webapi.ViewModels.Autenticacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace eAgenda.Webapi.Controllers
{
    [ApiController]
    public class eAgendaControllerBase : ControllerBase
    {
        private UsuarioTokenViewModel usuario;
        public UsuarioTokenViewModel UsuarioLogado
        {
            get {
                if (ExtaAutenticado())
                {
                    this.usuario= new UsuarioTokenViewModel();

                    var id = Request?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!string.IsNullOrEmpty(id))
                        this.usuario.Id = Guid.Parse(id);

                    var nome = Request?.HttpContext?.User?.FindFirst(ClaimTypes.GivenName)?.Value;

                    if (!string.IsNullOrEmpty(nome))
                        this.usuario.Nome = nome;

                    var email = Request?.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

                    if (!string.IsNullOrEmpty(email))
                        this.usuario.Email = email;
                    return usuario; 
                }
                return null;
            }
        }
        protected ActionResult InternalError<T>(Result<T> tarefaResult)
        {
            return StatusCode(500, new
            {
                sucesso = false,
                error = tarefaResult.Errors.Select(x => x.Message)
            });
        }
        protected static bool RegistroNaoEncontrado<T>(Result<T> tarefaResult)
        {
            return tarefaResult.Errors.Any(x => x.Message.Contains("não encontrada"));
        }
        protected ActionResult NotFound<T>(Result<T> tarefaResult)
        {
            return StatusCode(404, new
            {
                sucesso = false,
                error = tarefaResult.Errors.Select(x => x.Message)
            });
        }
        protected ActionResult BadRequest<T>(Result<T> tarefaResult)
        {
            return StatusCode(300, new
            {
                sucesso = false,
                error = tarefaResult.Errors.Select(x => x.Message)
            });
        }

        protected Guid ObtemId()
        {
            var id = Request?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(id)) return Guid.Empty;

            return Guid.Parse(id);
        }

        private bool ExtaAutenticado()
        {
            if (Request?.HttpContext?.User != null)
                return true;

            return false;
        }
    }
}
