using AutoMapper;
using eAgenda.Aplicacao.ModuloDespesa;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Webapi.ViewModels.Depesas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [HttpPost]
        public ActionResult<VisualizarDespesasViewModel> Inserir(FormDespesasViewModel contatoVM)
        {
            var contato = mapeadorDespesas.Map<Despesa>(contatoVM);

            contato.UsuarioId = UsuarioLogado.Id;

            var registroResult = servicoDespesa.Inserir(contato);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorDespesas.Map<VisualizarDespesasViewModel>(registroResult.Value)
            });
        }
        [HttpPut("{id:guid}")]
        public ActionResult<VisualizarDespesasViewModel> Editar(Guid id, FormDespesasViewModel despesaVM)
        {
            var depesaSelecionadaResult = servicoDespesa.SelecionarPorId(id);

            if (depesaSelecionadaResult.IsFailed &&  RegistroNaoEncontrado(depesaSelecionadaResult))
                return NotFound(depesaSelecionadaResult);

            var tarefa = mapeadorDespesas.Map(despesaVM, depesaSelecionadaResult.Value);

            var registroResult = servicoDespesa.Editar(tarefa);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorDespesas.Map<VisualizarDespesasViewModel>(registroResult.Value)
            });
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var despesaExcluir = servicoDespesa.SelecionarPorId(id);

            if (despesaExcluir.IsFailed && RegistroNaoEncontrado<Despesa>(despesaExcluir))
                return NotFound(despesaExcluir);

            var registroResult = servicoDespesa.Excluir(despesaExcluir.Value);


            if (registroResult.IsFailed)
                return InternalError<Categoria>(registroResult);

            return NoContent();
        }
    }
}
