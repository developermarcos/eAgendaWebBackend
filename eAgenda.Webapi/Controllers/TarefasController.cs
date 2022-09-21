using AutoMapper;
using eAgenda.Aplicacao.ModuloTarefa;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Infra.Configs;
using eAgenda.Infra.Orm;
using eAgenda.Infra.Orm.ModuloTarefa;
using eAgenda.Webapi.ViewModels.Tarefas;
using Microsoft.AspNetCore.Http;
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
                //definições de conversão

                //Da entidade para o view model
                config.CreateMap<Tarefa, ListarTarefaViewModel>()
                    .ForMember(destino => destino.Prioridade, opt => opt.MapFrom(origem => origem.Prioridade.GetDescription()))
                    .ForMember(destino => destino.Situacao, opt => opt.MapFrom(origen => origen.PercentualConcluido == 100 ? "Concluído" : "Pendente"));


                config.CreateMap<Tarefa, VisualizarTarefaViewModel>()
                    .ForMember(destino => destino.Prioridade, opt => opt.MapFrom(origem => origem.Prioridade.GetDescription()))
                    .ForMember(destino => destino.Situacao, opt => opt.MapFrom(origen => origen.PercentualConcluido == 100 ? "Concluído" : "Pendente"))
                    .ForMember(destino => destino.QuantidadeItens, opt => opt.MapFrom(origem => origem.Itens.Count));

                config.CreateMap<ItemTarefa, VisualizarItemTarefaViewModel>()
                    .ForMember(destino => destino.Status, opt => opt.MapFrom(origen => origen.Concluido ? "Concluído" : "Pendente"));

                //Do view model para entidade
                config.CreateMap<InserirTarefaViewModel, Tarefa>()
                    .ForMember(destino => destino.Itens, opt => opt.Ignore())
                    .AfterMap((viewModel, tarefa) =>
                    {
                        foreach (var itemVM in viewModel.Itens)
                        {
                            var item = new ItemTarefa();
                            item.Titulo = itemVM.Titulo;
                            tarefa.AdicionarItem(item);
                        }
                    });
                config.CreateMap<EditarTarefaViewModel, Tarefa>()
                    .ForMember(destino => destino.Itens, opt => opt.Ignore())
                    .AfterMap((viewModel, tarefa) =>
                    {
                        foreach (var itemVM in viewModel.Itens)
                        {
                            if (itemVM.Concluido)
                                tarefa.ConcluirItem(itemVM.Id);
                            else
                                tarefa.MarcarPendente(itemVM.Id);
                        }

                        foreach (var itemVM in viewModel.Itens)
                        {
                            if (itemVM.Status == StatusItemTarefa.Adicionado)
                            {
                                var item = new ItemTarefa(itemVM.Titulo);
                                tarefa.AdicionarItem(item);
                            }
                            else
                            {
                                tarefa.RemoverItem(itemVM.Id);
                            }
                        }
                    });

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
