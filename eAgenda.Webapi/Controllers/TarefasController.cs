using AutoMapper;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infra.Configs;
using eAgenda.Infra.Orm;
using eAgenda.Infra.Orm.ModuloTarefa;
using eAgenda.Webapi.CreateMap.AutoMapperCreateMap;
using eAgenda.Webapi.ViewModels.Tarefas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly ServicoTarefa servicoTarefa;
        private readonly IMapper mapeadorTarefas;

        public TarefasController()
        {
            var config = new ConfiguracaoAplicacaoeAgenda();
            var eAgendaDbContext = new eAgendaDbContext(config.ConnectionStrings);
            var repositorioTarefa = new RepositorioTarefaOrm(eAgendaDbContext);
            servicoTarefa = new ServicoTarefa(repositorioTarefa, eAgendaDbContext);

            var autoMappeConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<TarefaProfile>();

            });
            mapeadorTarefas = autoMappeConfig.CreateMapper();
        }
        [HttpGet]
        public List<ListarTarefaViewModel> SelecionarTodos()
        {
            var tarefaResult = servicoTarefa.SelecionarTodos(StatusTarefaEnum.Todos);

            if (tarefaResult.IsSuccess) return mapeadorTarefas.Map<List<ListarTarefaViewModel>>(tarefaResult.Value);

            return null;
        }
        
        [HttpGet("{id:guid}")]
        public VisualizarTarefaViewModel SelecionarPorId(Guid id)
        {
            var tarefaResult = servicoTarefa.SelecionarPorId(id);

            if (tarefaResult.IsSuccess) return mapeadorTarefas.Map<VisualizarTarefaViewModel>(tarefaResult.Value);

            return null;
        }

        [HttpPost]
        public InserirTarefaViewModel Inserir(InserirTarefaViewModel tarefaVM)
        {
            var tarefa = mapeadorTarefas.Map<Tarefa>(tarefaVM);
                        
            var tarefaResult = servicoTarefa.Inserir(tarefa);

            if (tarefaResult.IsSuccess) return tarefaVM;

            return null;
        }

        [HttpPut("{id:guid}")]
        public EditarTarefaViewModel Editar(Guid id, EditarTarefaViewModel tarefaVM)
        {
            var tarefaSelecionada = servicoTarefa.SelecionarPorId(id).Value;

            var tarefa = mapeadorTarefas.Map(tarefaVM, tarefaSelecionada);

            var tarefaResult = servicoTarefa.Editar(tarefa);

            if (tarefaResult.IsSuccess) return tarefaVM;

            return null;
        }
        [HttpDelete("{id:guid}")]
        public void Excluir(Guid id)
        {
            servicoTarefa.Excluir(id);
        }

    }
}
