using AutoMapper;
using eAgenda.Aplicacao.ModuloDespesa;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Webapi.ViewModels.Depesas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DespesasController : eAgendaControllerBase
    {
        private readonly ServicoDespesa servicoDespesa;
        private readonly IMapper mapeadorDespesas;

        public DespesasController(ServicoDespesa servicoDespesa, IMapper mapeadorDespesas)
        {
            this.servicoDespesa=servicoDespesa;
            this.mapeadorDespesas=mapeadorDespesas;
        }

        [HttpGet]
        public ActionResult<List<ListarDespesasViewModel>> SelecionarTodos()
        {
            var registroResult = servicoDespesa.SelecionarTodos();

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                //dados = registroResult.Value
                dados = mapeadorDespesas.Map<List<ListarDespesasViewModel>>(registroResult.Value)
            });
        }
        [HttpGet("{id:guid}")]
        public ActionResult<VisualizarDespesasViewModel> SelecionarPorId(Guid id)
        {
            var registroResult = servicoDespesa.SelecionarPorId(id);

            if (registroResult.IsFailed && RegistroNaoEncontrado(registroResult))
                return NotFound(registroResult);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorDespesas.Map<VisualizarDespesasViewModel>(registroResult.Value)
            });
        }
    }
}
