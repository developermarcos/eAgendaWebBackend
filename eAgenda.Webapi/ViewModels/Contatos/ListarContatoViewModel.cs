using System;

namespace eAgenda.Webapi.ViewModels.Contatos
{
    public class ListarContatoViewModel
    {

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Empresa { get; set; }
    }
}
