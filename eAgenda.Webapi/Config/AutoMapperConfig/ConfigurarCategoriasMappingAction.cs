using AutoMapper;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Webapi.ViewModels.Depesas;

namespace eAgenda.Webapi.Config.AutoMapperConfig
{
    public class ConfigurarCategoriasMappingAction : IMappingAction<FormDespesasViewModel, Despesa>
    {
        private readonly IRepositorioCategoria repositorioCategoria;

        public ConfigurarCategoriasMappingAction(IRepositorioCategoria repositorioCategoria)
        {
            this.repositorioCategoria = repositorioCategoria;
        }

        public void Process(FormDespesasViewModel despesaVM, Despesa despesa, ResolutionContext context)
        {
            foreach (var categoriaVM in despesaVM.CategoriasSelecionadas)
            {
                var categoria = repositorioCategoria.SelecionarPorId(categoriaVM.Id);

                if (categoriaVM.Selecionada)
                    despesa.AtribuirCategoria(categoria);
                else
                    despesa.RemoverCategoria(categoria);
            }
        }
    }
}
