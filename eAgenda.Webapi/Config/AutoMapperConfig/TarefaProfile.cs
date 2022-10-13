using AutoMapper;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Webapi.ViewModels.Tarefas;

namespace eAgenda.Webapi.CreateMap.AutoMapperCreateMap
{
    public class TarefaProfile : Profile
    {
        public TarefaProfile()
        {
            //definições de conversão

            ConverterEntidadeParaViewModel();
            ConverterViewModelParaEntidade();
        }

        private void ConverterViewModelParaEntidade()
        {
            CreateMap<InserirTarefaViewModel, Tarefa>()
                .ForMember(destino => destino.Itens, opt => opt.Ignore())
                .ForMember(destino => destino.Id, opt => opt.Ignore())
                .AfterMap((viewModel, tarefa) =>
                {
                    foreach (var itemVM in viewModel.Itens)
                    {
                        var item = new ItemTarefa();
                        item.Titulo = itemVM.Titulo;
                        tarefa.AdicionarItem(item);
                    }
                });
            CreateMap<EditarTarefaViewModel, Tarefa>()
                .ForMember(destino => destino.Itens, opt => opt.Ignore())
                .ForMember(destino => destino.Id, opt => opt.Ignore())
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
                        if (itemVM.Status == StatusItemTarefa.Removido)
                        {
                            tarefa.RemoverItem(itemVM.Id);
                        }
                    }
                });
        }

        private void ConverterEntidadeParaViewModel()
        {
            CreateMap<Tarefa, ListarTarefaViewModel>()
                .ForMember(destino => destino.Prioridade, opt => opt.MapFrom(origem => origem.Prioridade.GetDescription()))
                .ForMember(destino => destino.Situacao, opt => opt.MapFrom(origen => origen.PercentualConcluido == 100 ? "Concluído" : "Pendente"));


            CreateMap<Tarefa, VisualizarTarefaViewModel>()
                .ForMember(destino => destino.Prioridade, opt => opt.MapFrom(origem => origem.Prioridade.GetDescription()))
                .ForMember(destino => destino.Situacao, opt => opt.MapFrom(origen => origen.PercentualConcluido == 100 ? "Concluído" : "Pendente"))
                .ForMember(destino => destino.QuantidadeItens, opt => opt.MapFrom(origem => origem.Itens.Count));

            CreateMap<ItemTarefa, VisualizarItemTarefaViewModel>()
                .ForMember(destino => destino.Status, opt => opt.MapFrom(origen => origen.Concluido ? "Concluído" : "Pendente"));

            CreateMap<Tarefa, FormTarefaViewModel>();

            CreateMap<ItemTarefa, FormItemTarefaViewModel>();
        }
    }
}
