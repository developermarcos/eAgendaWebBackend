using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace eAgenda.Webapi.Controllers
{
    [ApiController]
    public class eAgendaControllerBase : ControllerBase
    {
        #region métodos privados
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
        #endregion
    }
}
