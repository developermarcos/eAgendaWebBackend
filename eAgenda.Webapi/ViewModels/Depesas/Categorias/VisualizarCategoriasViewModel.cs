using eAgenda.Dominio.ModuloDespesa;
using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.ViewModels.Depesas.Categorias
{
    public class VisualizarCategoriasViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public List<Despesa> Despesas { get; set; }
    }
}
