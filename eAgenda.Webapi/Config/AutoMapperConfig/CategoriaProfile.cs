using AutoMapper;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Webapi.ViewModels.Depesas.Categorias;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            //definições de conversão

            ConverterEntidadeParaViewModel();
            ConverterViewModelParaEntidade();
        }

        private void ConverterViewModelParaEntidade()
        {
            //CreateMap<InserirContatoViewModel, Contato>();
            //CreateMap<EditarContatoViewModel, Contato>();
        }

        private void ConverterEntidadeParaViewModel()
        {
            CreateMap<Categoria, ListarCategoriasViewModel>();
            //CreateMap<Contato, VisualizarContatoViewModel>();

        }
    }
}
