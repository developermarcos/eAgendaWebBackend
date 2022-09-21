using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.ViewModels.Tarefas
{
    public class VisualizarTarefaViewModel
    {
        public VisualizarTarefaViewModel()
        {
            Itens = new List<VisualizarItemTarefaViewModel>();
        }
        public string Titulo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string Prioridade { get; set; }
        public string Situacao { get; set; }
        public int QuantidadeItens { get; set; }
        public int PercentualConcluido { get; set; }

        public List<VisualizarItemTarefaViewModel> Itens { get; set; }
    }
}
