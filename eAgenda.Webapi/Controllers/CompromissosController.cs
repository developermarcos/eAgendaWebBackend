using AutoMapper;
using eAgenda.Aplicacao.ModuloCompromisso;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Webapi.ViewModels.Compromisso;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompromissosController : eAgendaControllerBase
    {
        private readonly ServicoCompromisso servicoCompromisso;
        private readonly IMapper mapeadorCompromisso;

        public CompromissosController(ServicoCompromisso servicoCompromisso, IMapper mapeadorCompromisso)
        {
            this.servicoCompromisso=servicoCompromisso;
            this.mapeadorCompromisso=mapeadorCompromisso;
        }

        [HttpGet]
        public ActionResult<List<ListarCompromissosViewModel>> SelecionarTodos()
        {

            var registroResult = servicoCompromisso.SelecionarTodos();

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCompromisso.Map<List<ListarCompromissosViewModel>>(registroResult.Value)
            });
        }
        [HttpGet("{id:guid}")]
        public ActionResult<VisualizarCompromissoViewModel> SelecionarPorId(Guid id)
        {
            var registroResult = servicoCompromisso.SelecionarPorId(id);

            if (registroResult.IsFailed && RegistroNaoEncontrado(registroResult))
                return NotFound(registroResult);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCompromisso.Map<VisualizarCompromissoViewModel>(registroResult.Value)
            });
        }
        [HttpPost]
        public ActionResult<FormCompromissoViewModel> Inserir(FormCompromissoViewModel CompromissoVM)
        {
            var compromisso = mapeadorCompromisso.Map<Compromisso>(CompromissoVM);
            
            compromisso.UsuarioId = UsuarioLogado.Id;

            var registroResult = servicoCompromisso.Inserir(compromisso);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCompromisso.Map<VisualizarCompromissoViewModel>(registroResult.Value)
            });
        }
        [HttpPut("{id:guid}")]
        public ActionResult<FormCompromissoViewModel> Editar(Guid id, FormCompromissoViewModel compromissoVM)
        {
            var compromissoSelecionadaResult = servicoCompromisso.SelecionarPorId(id);

            if (compromissoSelecionadaResult.IsFailed &&  RegistroNaoEncontrado(compromissoSelecionadaResult))
                return NotFound(compromissoSelecionadaResult);

            var tarefa = mapeadorCompromisso.Map(compromissoVM, compromissoSelecionadaResult.Value);

            var registroResult = servicoCompromisso.Editar(tarefa);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCompromisso.Map<VisualizarCompromissoViewModel>(registroResult.Value)
            });
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var compromissoExcluir = servicoCompromisso.SelecionarPorId(id);
                        
            if (compromissoExcluir.IsFailed && RegistroNaoEncontrado<Compromisso>(compromissoExcluir))
                return NotFound(compromissoExcluir);

            var registroResult = servicoCompromisso.Excluir(compromissoExcluir.Value);


            if (registroResult.IsFailed)
                return InternalError<Compromisso>(registroResult);

            return NoContent();
        }
    }
}
