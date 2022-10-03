using AutoMapper;
using eAgenda.Aplicacao.ModuloDespesa;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Webapi.ViewModels.Depesas.Categorias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriasController : eAgendaControllerBase
    {
        private readonly ServicoCategoria servicoCategoria;
        private readonly IMapper mapeadorCategoria;

        public CategoriasController(ServicoCategoria servicoCategoria, IMapper mapeadorCategoria)
        {
            this.servicoCategoria=servicoCategoria;
            this.mapeadorCategoria=mapeadorCategoria;
        }

        public IMapper MapeadorCategoria { get; }

        [HttpGet]
        public ActionResult<List<ListarCategoriasViewModel>> SelecionarTodos()
        {

            var registroResult = servicoCategoria.SelecionarTodos();

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCategoria.Map<List<ListarCategoriasViewModel>>(registroResult.Value)
            });
        }

        [HttpGet("{id:guid}")]
        public ActionResult<VisualizarCategoriasViewModel> SelecionarPorId(Guid id)
        {
            var registroResult = servicoCategoria.SelecionarPorId(id, true);

            if (registroResult.IsFailed && RegistroNaoEncontrado(registroResult))
                return NotFound(registroResult);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCategoria.Map<VisualizarCategoriasViewModel>(registroResult.Value)
            });
        }
        [HttpPost]
        public ActionResult<VisualizarCategoriasViewModel> Inserir(FormCategoriaViewModel categoriaVM)
        {
            var categoria = mapeadorCategoria.Map<Categoria>(categoriaVM);

            categoria.UsuarioId = UsuarioLogado.Id;

            var registroResult = servicoCategoria.Inserir(categoria);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCategoria.Map<VisualizarCategoriasViewModel>(registroResult.Value)
            });
        }
        [HttpPut("{id:guid}")]
        public ActionResult<VisualizarCategoriasViewModel> Editar(Guid id, FormCategoriaViewModel categoriaVM)
        {
            var categoriaSelecionadaResult = servicoCategoria.SelecionarPorId(id);

            if (categoriaSelecionadaResult.IsFailed &&  RegistroNaoEncontrado(categoriaSelecionadaResult))
                return NotFound(categoriaSelecionadaResult);

            var tarefa = mapeadorCategoria.Map(categoriaVM, categoriaSelecionadaResult.Value);

            var registroResult = servicoCategoria.Editar(tarefa);

            if (registroResult.IsFailed)
                return InternalError(registroResult);

            return Ok(new
            {
                sucesso = true,
                dados = mapeadorCategoria.Map<VisualizarCategoriasViewModel>(registroResult.Value)
            });
        }

        [HttpDelete("{id:guid}")]
        public ActionResult Excluir(Guid id)
        {
            var categoriaExcluir = servicoCategoria.SelecionarPorId(id);

            if (categoriaExcluir.IsFailed && RegistroNaoEncontrado<Categoria>(categoriaExcluir))
                return NotFound(categoriaExcluir);

            var registroResult = servicoCategoria.Excluir(categoriaExcluir.Value);


            if (registroResult.IsFailed)
                return InternalError<Categoria>(registroResult);

            return NoContent();
        }
    }
}
