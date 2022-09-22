using AutoMapper;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Webapi.CreateMap.AutoMapperCreateMap;
using eAgenda.Webapi.ViewModels.Tarefas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
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
            {
                return StatusCode(500, new
                {
                    sucesso = false,
                    error = tarefaResult.Errors.Select(x => x.Message)
                }) ;
            }
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
            if (tarefaResult.Errors.Any(x => x.Message.Contains("não encontrada")))
            {
                return NotFound(new
                {
                    sucesso = false,
                    error = tarefaResult.Errors.Select(x => x.Message)
                });
            }
            if (tarefaResult.IsFailed)
            {
                return StatusCode(500, new
                {
                    sucesso = false,
                    error = tarefaResult.Errors.Select(x => x.Message)
                });
            }
            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<VisualizarTarefaViewModel>(tarefaResult.Value)
            });
        }

        [HttpPost]
        public ActionResult<InserirTarefaViewModel> Inserir(InserirTarefaViewModel tarefaVM)
        {
            var listaErros = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

            if (listaErros.Any())
            {
                return BadRequest(new
                {
                    sucesso = false,
                    erros = listaErros.ToList()
                });
            }
            var tarefa = mapeadorTarefas.Map<Tarefa>(tarefaVM);
                        
            var tarefaResult = servicoTarefa.Inserir(tarefa);

            if (tarefaResult.IsFailed)
            {
                return StatusCode(500, new
                {
                    sucesso = false,
                    error = tarefaResult.Errors.Select(x => x.Message)
                });
            }
            return Ok(new
            {
                sucesso = true,
                dados = mapeadorTarefas.Map<VisualizarTarefaViewModel>(tarefaResult.Value)
            });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<EditarTarefaViewModel> Editar(Guid id, EditarTarefaViewModel tarefaVM)
        {
            var listaErros = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

            if (listaErros.Any())
            {
                return BadRequest(new
                {
                    sucesso = false,
                    erros = listaErros.ToList()
                });
            }
            var tarefaSelecionadaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaSelecionadaResult.Errors.Any(x => x.Message.Contains("não encontrada")))
            {
                return NotFound(new
                {
                    sucesso = false,
                    error = tarefaSelecionadaResult.Errors.Select(x => x.Message)
                });
            }

            var tarefa = mapeadorTarefas.Map(tarefaVM, tarefaSelecionadaResult.Value);

            var tarefaResult = servicoTarefa.Editar(tarefa);

            if (tarefaResult.IsFailed)
            {
                return StatusCode(500, new
                {
                    sucesso = false,
                    error = tarefaResult.Errors.Select(x => x.Message)
                });
            }
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

            if (tarefaResult.Errors.Any(x => x.Message.Contains("não encontrada")))
            {
                return NotFound(new
                {
                    sucesso = false,
                    error = tarefaResult.Errors.Select(x => x.Message)
                });
            }
            if (tarefaResult.IsFailed)
            {
                return StatusCode(500, new
                {
                    sucesso = false,
                    error = tarefaResult.Errors.Select(x => x.Message)
                });
            }
            return NoContent();

        }

    }
}
