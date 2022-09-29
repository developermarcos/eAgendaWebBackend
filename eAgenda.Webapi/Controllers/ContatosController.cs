using AutoMapper;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Dominio.ModuloContato;
using eAgenda.Infra.Configs;
using eAgenda.Infra.Orm;
using eAgenda.Infra.Orm.ModuloContato;
using eAgenda.Webapi.ViewModels.Contatos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContatosController : eAgendaControllerBase
    {
        private readonly ServicoContato servicoContato;
        private readonly IMapper mapeadorContatos;
        
        public ContatosController(ServicoContato servicoContato, IMapper mapeadorContatos)
        {
            this.servicoContato=servicoContato;
            this.mapeadorContatos=mapeadorContatos;
        }

        [HttpGet]
        public ActionResult<List<ListarContatoViewModel>> SelecionarTodos()
        {
            var registroResult = servicoContato.SelecionarTodos();

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorContatos.Map<List<ListarContatoViewModel>>(registroResult.Value)
            });
        }

        [HttpGet("{id:guid}")]
        public ActionResult<VisualizarContatoViewModel> SelecionarPorId(Guid id)
        {
            var registroResult = servicoContato.SelecionarPorId(id);

            if (registroResult.IsFailed && RegistroNaoEncontrado(registroResult))
                return NotFound(registroResult);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorContatos.Map<VisualizarContatoViewModel>(registroResult.Value)
            });
        }

        [HttpPost]
        public ActionResult<InserirContatoViewModel> Inserir(InserirContatoViewModel contatoVM)
        {
            var contato = mapeadorContatos.Map<Contato>(contatoVM);

            var registroResult = servicoContato.Inserir(contato);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorContatos.Map<VisualizarContatoViewModel>(registroResult.Value)
            });
        }

        [HttpPut("{id:guid}")]
        public ActionResult<EditarContatoViewModel> Editar(Guid id, EditarContatoViewModel contatoVM)
        {
            var contatoSelecionadaResult = servicoContato.SelecionarPorId(id);

            if (contatoSelecionadaResult.IsFailed &&  RegistroNaoEncontrado(contatoSelecionadaResult))
                return NotFound(contatoSelecionadaResult);
            
            var contato = mapeadorContatos.Map(contatoVM, contatoSelecionadaResult.Value);

            var contatoResult = servicoContato.Editar(contato);

            if (contatoResult.IsFailed)
                return InternalError(contatoResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorContatos.Map<VisualizarContatoViewModel>(contatoResult.Value)
            });
        }
        [HttpDelete]
        public ActionResult Excluir(Contato contato)
        {
            var registroResult = servicoContato.Excluir(contato);

            if (registroResult.IsFailed && RegistroNaoEncontrado<Contato>(registroResult))
                return NotFound(registroResult);

            if (registroResult.IsFailed)
                return InternalError<Contato>(registroResult);

            return NoContent();
        }
    }
}
