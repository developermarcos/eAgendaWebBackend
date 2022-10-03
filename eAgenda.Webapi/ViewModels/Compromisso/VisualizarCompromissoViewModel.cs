using eAgenda.Webapi.ViewModels.Contatos;

namespace eAgenda.Webapi.ViewModels.Compromisso
{
    public class VisualizarCompromissoViewModel
    {
        public string Assunto { get; set; }
        public string ?Local { get; set; }
        public string TipoLocal { get; set; }
        public string ?Link { get; set; }
        public string Data { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }

        public VisualizarContatoViewModel Contato {get; set;}
    }
}
