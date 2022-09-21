using eAgenda.Dominio.ModuloTarefa;
using System;

namespace eAgenda.Webapi.ViewModels.Tarefas
{
    public class FormItemTarefaViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public bool Concluido { get; set; }
        public StatusItemTarefa Status { get; set; }
    }
}
