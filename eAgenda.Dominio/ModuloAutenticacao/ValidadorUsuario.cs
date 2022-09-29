using FluentValidation;
using FluentValidation.Validators;

namespace eAgenda.Dominio.ModuloAutenticacao
{
    public class ValidadorUsuario : AbstractValidator<Usuario>
    {
        public ValidadorUsuario()
        {
            RuleFor(x => x.Nome)
                .NotNull().WithMessage("Campo nome é obrigatório")
                .NotEmpty().WithMessage("O campo nome é obrigatório");
        }
    }
}
