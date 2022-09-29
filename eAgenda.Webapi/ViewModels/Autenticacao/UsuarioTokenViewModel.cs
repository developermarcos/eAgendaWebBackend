using System;

namespace eAgenda.Webapi.ViewModels.Autenticacao
{
    public class UsuarioTokenViewModel
    {
        public Guid Id { get; internal set; }
        public string Nome { get; internal set; }
        public string Email { get; internal set; }
    }
}
