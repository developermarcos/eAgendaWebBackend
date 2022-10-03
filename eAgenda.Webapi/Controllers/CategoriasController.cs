using AutoMapper;
using eAgenda.Aplicacao.ModuloDespesa;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Webapi.ViewModels.Depesas.Categorias;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : eAgendaControllerBase
    {
        private readonly ServicoCategoria servicoCategoria;
        private readonly IMapper mapeadorCategoria;

        public CategoriasController(ServicoCategoria servicoCategoria, IMapper mapeadorCategoria)
        {
            this.servicoCategoria=servicoCategoria;
            this.mapeadorCategoria=mapeadorCategoria;
        }

        public IMapper MapeadorCategoria { get; }

        [HttpGet]
        public ActionResult<List<ListarCategoriasViewModel>> SelecionarTodos()
        {

            var registroResult = servicoCategoria.SelecionarTodos();

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCategoria.Map<List<ListarCategoriasViewModel>>(registroResult.Value)
            });
        }
    }
}
