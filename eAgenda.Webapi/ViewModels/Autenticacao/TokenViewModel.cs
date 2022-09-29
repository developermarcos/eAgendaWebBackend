namespace eAgenda.Webapi.ViewModels.Autenticacao
{
    public class TokenViewModel
    {
        public string Chave { get; internal set; }
        public UsuarioTokenViewModel UsuarioToken { get; internal set; }
    }
}
