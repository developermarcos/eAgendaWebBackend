using System.ComponentModel.DataAnnotations;

namespace eAgenda.Webapi.ViewModels.Contatos
{
    public abstract class FormContatoViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Telefone { get; set; }
        public string Empresa { get; set; }
        public string Cargo { get; set; }
    }
    public class InserirContatoViewModel : FormContatoViewModel
    {

    }
    public class EditarContatoViewModel : FormContatoViewModel
    {

    }
}
