using eAgenda.Dominio.ModuloAutenticacao;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eAgenda.Aplicacao.ModuloAutenticacao
{
    public class ServicoAutenticacao : ServicoBase<Usuario, ValidadorUsuario>
    {
        private readonly UserManager<Usuario> userManager;
        private readonly SignInManager<Usuario> signInManager;

        public ServicoAutenticacao(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            this.userManager=userManager;
            this.signInManager=signInManager;
        }
        public async Task<Result<Usuario>> Registrarusuario(Usuario usuario, string senha)
        {
            Log.Logger.Debug("Tentando registrar usuário {@u}", usuario);
            
            var resultado = Validar(usuario);
            if (resultado.IsFailed)
                return Result.Fail(resultado.Errors);
            
            try
            {
                var usuarioResult = await userManager.CreateAsync(usuario, senha);

                if(usuarioResult.Succeeded == false)
                {
                    var erros = usuarioResult.Errors.Select(identityErro => new Error(identityErro.Description));
                    return Result.Fail(erros);
                }

                Log.Logger.Information("Usuario {UsuarioId} inserido com sucesso", usuario.Id);

            }
            catch (Exception ex)
            {
                string msgErro = "Falha no sistema ao tentar registrar o Usuário";

                Log.Logger.Error(ex, msgErro + " {UsuarioId} ", usuario.Id);

                return Result.Fail(msgErro);
            }

            return Result.Ok();

        }

        public async Task<Result<Usuario>> AutenticarUsuario(string email, string senha)
        {
            Log.Logger.Debug("Tentando autenticar usuário {@e}", email);

            SignInResult signResult = await signInManager.PasswordSignInAsync(email, senha, false, true);

            if (signResult.Succeeded == false && signResult.IsLockedOut)
            {
                string msgErro = "usuário bloqueado!";
                Log.Logger.Debug($"{msgErro} Email: {email}");
                return Result.Fail(msgErro);
            }

            if(signResult.Succeeded == false)
            {
                string msgErro = "Email ou Senha inválido!";
                Log.Logger.Debug($"{msgErro} Email: {email}");
                return Result.Fail(msgErro);
            }

            Usuario usuario = await userManager.FindByEmailAsync(email);

            return Result.Ok(usuario);
        }

        public async Task<Result<Usuario>> Sair(string email)
        {
            await signInManager.SignOutAsync();
            Log.Logger.Debug("Sessão do usuário {@email} removida...", email);
            return Result.Ok();
        }
    }
}
