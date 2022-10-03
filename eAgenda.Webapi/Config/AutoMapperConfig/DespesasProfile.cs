using AutoMapper;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Webapi.ViewModels.Depesas;
using System.Linq;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class DespesasProfile : Profile
    {
        public DespesasProfile()
        {
            //definições de conversão

            ConverterEntidadeParaViewModel();
            ConverterViewModelParaEntidade();
        }

        private void ConverterViewModelParaEntidade()
        {
            //CreateMap<FormsDespesaViewModel, Despesa>()
            //    .ForMember(destino => destino.UsuarioId, opt => opt.MapFrom<UsuarioResolver>())
            //    .AfterMap<ConfigurarCategoriasMappingAction>();
        }

        private void ConverterEntidadeParaViewModel()
        {
            CreateMap<Despesa, ListarDespesasViewModel>();
            
            CreateMap<Despesa, VisualizarDespesasViewModel>()
                .ForMember(destino => destino.FormaPagamento, opt => opt.MapFrom(origem => origem.FormaPagamento.GetDescription()))
                .ForMember(destino => destino.Categorias, opt => opt.MapFrom(origem => origem.Categorias.Select(x => x.Titulo)));

        }
    }
}
