﻿using eAgenda.Dominio.ModuloTarefa;
using System.Collections.Generic;

namespace eAgenda.Webapi.ViewModels.Tarefas
{
    public class FormTarefaViewModel
    {
        public string Titulo { get; set; }
        public PrioridadeTarefaEnum Prioridade { get; set; }
        public List<FormItemTarefaViewModel> Itens{ get; set; }
    }

    public class InserirTarefaViewModel : FormTarefaViewModel
    {

    }
    public class EditarTarefaViewModel : FormTarefaViewModel
    {

    }
}
