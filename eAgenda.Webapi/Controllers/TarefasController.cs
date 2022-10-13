using AutoMapper;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Webapi.ViewModels.Tarefas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Taikandi;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TarefasController : eAgendaControllerBase
    {
        private readonly ServicoTarefa servicoTarefa;
        private readonly IMapper mapeadorTarefas;

        public TarefasController(ServicoTarefa servicoTarefa, IMapper mapeadorTarefas)
        {
            this.servicoTarefa = servicoTarefa;
            this.mapeadorTarefas=mapeadorTarefas;
        }
        [HttpGet]
        public ActionResult<List<ListarTarefaViewModel>> SelecionarTodos()
        {
            var registroResult = servicoTarefa.SelecionarTodos(StatusTarefaEnum.Todos, UsuarioLogado.Id);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<List<ListarTarefaViewModel>>(registroResult.Value)
            });
        }

        [HttpGet("visualizao-completa/{id:guid}")]
        public ActionResult<VisualizarTarefaViewModel> SelecionarPorId(Guid id)
        {
            var registroResult = servicoTarefa.SelecionarPorId(id);

            if (registroResult.IsFailed && RegistroNaoEncontrado(registroResult))
                return NotFound(registroResult);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<VisualizarTarefaViewModel>(registroResult.Value)
            });
        }

        [HttpGet("{id:guid}")]
        public ActionResult<FormTarefaViewModel> SelecionarTarefaPorId(Guid id)
        {
            var tarefaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaResult.IsFailed && RegistroNaoEncontrado(tarefaResult))
                return NotFound(tarefaResult);

            if (tarefaResult.IsFailed)
                return InternalError(tarefaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<FormTarefaViewModel>(tarefaResult.Value)
            });
        }

        [HttpPost]
        public ActionResult<InserirTarefaViewModel> Inserir(InserirTarefaViewModel tarefaVM)
        {
            var tarefa = mapeadorTarefas.Map<Tarefa>(tarefaVM);

            tarefa.UsuarioId = UsuarioLogado.Id;

            var registroResult = servicoTarefa.Inserir(tarefa);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<VisualizarTarefaViewModel>(registroResult.Value)
            });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<EditarTarefaViewModel> Editar(Guid id, EditarTarefaViewModel tarefaVM)
        {
            var tarefaSelecionadaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaSelecionadaResult.IsFailed &&  RegistroNaoEncontrado(tarefaSelecionadaResult))
                return NotFound(tarefaSelecionadaResult);

            var tarefa = mapeadorTarefas.Map(tarefaVM, tarefaSelecionadaResult.Value);

            var registroResult = servicoTarefa.Editar(tarefa);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<VisualizarTarefaViewModel>(registroResult.Value)
            });
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var registroResult = servicoTarefa.Excluir(id);

            if (registroResult.IsFailed && RegistroNaoEncontrado<Tarefa>(registroResult))
                return NotFound(registroResult);

            if (registroResult.IsFailed)
                return InternalError<Tarefa>(registroResult);

            return NoContent();
        }

    }
}
