using eAgenda.Dominio.ModuloTarefa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eAgenda.Webapi.ViewModels.Tarefas
{
    public class FormTarefaViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public PrioridadeTarefaEnum Prioridade { get; set; }
        public List<FormItemTarefaViewModel> Itens{ get; set; }
        public FormTarefaViewModel()
        {
            Itens = new List<FormItemTarefaViewModel>();
        }
    }

    public class InserirTarefaViewModel : FormTarefaViewModel
    {

    }
    public class EditarTarefaViewModel : FormTarefaViewModel
    {

    }
}
