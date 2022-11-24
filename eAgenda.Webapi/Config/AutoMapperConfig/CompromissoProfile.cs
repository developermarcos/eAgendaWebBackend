using AutoMapper;
using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Webapi.ViewModels.Compromisso;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class CompromissoProfile : Profile
    {
        public CompromissoProfile()
        {

            //definições de conversão

            ConverterEntidadeParaViewModel();
            ConverterViewModelParaEntidade();
        }

        private void ConverterViewModelParaEntidade()
        {
            CreateMap<FormCompromissoViewModel, Compromisso>()
                .ForMember(destino => destino.Id, opt => opt.Ignore())
                .ForMember(destino => destino.Link, opt => opt.Ignore())
                .ForMember(destino => destino.Local, opt => opt.Ignore())
                .ForMember(destino => destino.TipoLocal, opt => opt.Ignore())
                .AfterMap((viewModel, compromisso) =>
                {
                    if (viewModel.TipoLocal == TipoLocalizacaoCompromissoEnum.Presencial)
                        compromisso.Local = viewModel.Local;

                    if (viewModel.TipoLocal == TipoLocalizacaoCompromissoEnum.Remoto)
                        compromisso.Link = viewModel.Link;
                });
        }

        private void ConverterEntidadeParaViewModel()
        {
            CreateMap<Compromisso, ListarCompromissosViewModel>();

            CreateMap<Compromisso, VisualizarCompromissoViewModel>()
                .ForMember(destino => destino.Link, opt => opt.Ignore())
                .ForMember(destino => destino.Local, opt => opt.Ignore())
                .ForMember(destino => destino.TipoLocal, opt => opt.MapFrom(origem => origem.TipoLocal.GetDescription()))
                .AfterMap((compromisso, viewModel) =>
                {
                    if (compromisso.TipoLocal == TipoLocalizacaoCompromissoEnum.Presencial)
                        viewModel.Local = compromisso.Local;

                    if (compromisso.TipoLocal == TipoLocalizacaoCompromissoEnum.Remoto)
                        viewModel.Link = compromisso.Link;
                });
        }
    }
}
