using eAgenda.Dominio.Compartilhado;
using System;

namespace eAgenda.Dominio.ModuloDespesa
{
    public interface IRepositorioCategoria : IRepositorio<Categoria>
    {
        Categoria SelecionarPorId(Guid id, bool carregarDespesas);
    }
}
