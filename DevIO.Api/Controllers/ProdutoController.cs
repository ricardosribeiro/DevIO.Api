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
            IMapper mapper)
        {
            _produtoService = produtoService;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> ObterTodos()
        {
            var produtos = _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
            if (produtos == null) return NotFound();

            return Ok();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
        {
            var produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
            if (produto == null) return NotFound();

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> Adicionar([FromBody] ProdutoViewModel produtoViewModel)
        {
            if (produtoViewModel == null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Atualizar(Guid id, [FromBody] ProdutoViewModel produtoViewModel)
        {
            if (produtoViewModel.Id != id) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoViewModel));

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Remover(Guid id)
        {
            var produto = _produtoRepository.ObterPorId(id);

            if (produto == null) return NotFound();

            await _produtoService.Remover(id);

            return Ok();
        }
    }
}
