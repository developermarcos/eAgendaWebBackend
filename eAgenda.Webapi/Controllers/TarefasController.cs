using AutoMapper;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Webapi.ViewModels.Tarefas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var tarefaResult = servicoTarefa.SelecionarTodos(StatusTarefaEnum.Todos);

            if (tarefaResult.IsFailed)
                return InternalError(tarefaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<List<ListarTarefaViewModel>>(tarefaResult.Value)
            });
        }

        [HttpGet("{id:guid}")]
        public ActionResult<VisualizarTarefaViewModel> SelecionarPorId(Guid id)
        {
            var tarefaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaResult.IsFailed && RegistroNaoEncontrado(tarefaResult))
                return NotFound(tarefaResult);

            if (tarefaResult.IsFailed)
                return InternalError(tarefaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<VisualizarTarefaViewModel>(tarefaResult.Value)
            });
        }

        [HttpPost]
        public ActionResult<InserirTarefaViewModel> Inserir(InserirTarefaViewModel tarefaVM)
        {
            var tarefa = mapeadorTarefas.Map<Tarefa>(tarefaVM);

            var tarefaResult = servicoTarefa.Inserir(tarefa);

            if (tarefaResult.IsFailed)
                return InternalError(tarefaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<VisualizarTarefaViewModel>(tarefaResult.Value)
            });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<EditarTarefaViewModel> Editar(Guid id, EditarTarefaViewModel tarefaVM)
        {
            var tarefaSelecionadaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaSelecionadaResult.IsFailed &&  RegistroNaoEncontrado(tarefaSelecionadaResult))
                return NotFound(tarefaSelecionadaResult);

            var tarefa = mapeadorTarefas.Map(tarefaVM, tarefaSelecionadaResult.Value);

            var tarefaResult = servicoTarefa.Editar(tarefa);

            if (tarefaResult.IsFailed)
                return InternalError(tarefaResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<VisualizarTarefaViewModel>(tarefaResult.Value)
            });
        }
        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var tarefaResult = servicoTarefa.Excluir(id);

            if (tarefaResult.IsFailed && RegistroNaoEncontrado<Tarefa>(tarefaResult))
                return NotFound(tarefaResult);

            if (tarefaResult.IsFailed)
                return InternalError<Tarefa>(tarefaResult);

            return NoContent();
        }

    }
}
