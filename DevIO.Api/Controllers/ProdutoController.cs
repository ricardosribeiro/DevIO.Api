using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    public class ProdutoController : MainController
    {
        readonly IProdutoService _produtoService;
        readonly IProdutoRepository _produtoRepository;
        readonly IMapper _mapper;

        public ProdutoController(
            IProdutoService produtoService,
            IProdutoRepository produtoRepository,
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _produtoService = produtoService;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> ObterTodos()
        {
            IEnumerable<ProdutoViewModel> produtosViewModel = await ObterProdutos();
            if (produtosViewModel == null) return NotFound();

            return Ok(produtosViewModel);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
        {
            var produtoViewModel = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
            if (produtoViewModel == null) return NotFound();

            return Ok(produtoViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> Adicionar([FromBody] ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Atualizar(Guid id, [FromBody] ProdutoViewModel produtoViewModel)
        {
            if (produtoViewModel.Id != id)
            {
                NotificaErro("O Id informado não corresponde ao produto.");
                CustomResponse(produtoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Remover(Guid id)
        {
            var produtoViewModel = await ObterProdutoPorId(id);

            if (produtoViewModel == null) return NotFound();

            await _produtoService.Remover(id);

            return CustomResponse();
        }

        #region Métodos Auxiliares
        private async Task<ProdutoViewModel> ObterProdutoPorId(Guid id)
        {
            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
        }

        private async Task<IEnumerable<ProdutoViewModel>> ObterProdutos()
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
        }

        #endregion
    }
}
