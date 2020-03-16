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
    public class FornecedorController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        public FornecedorController(
            IFornecedorRepository fornecedorRepository,
            IFornecedorService fornecedorService,
            IMapper mapper)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterTodos()
        {
            var fornecedores = await ObterTodosFornecedores();

            if (fornecedores == null) return NotFound();

            return Ok(fornecedores);

        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {
            FornecedorViewModel fornecedor = await ObterFornecedorPorId(id);
            if (fornecedor == null) return NotFound();

            return Ok(fornecedor);
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar([FromBody] FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var retorno = await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));

            if (!retorno) return BadRequest();

            return Ok(fornecedorViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, [FromBody] FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return BadRequest();

            if (!ModelState.IsValid) return BadRequest();

            var retorno = await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));

            if (!retorno) return BadRequest();

            return Ok(fornecedorViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remover(Guid id)
        {
            var retorno = await _fornecedorService.Remover(id);

            if (!retorno) return BadRequest();

            return Ok();
        }

        private async Task<FornecedorViewModel> ObterFornecedorPorId(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterPorId(id));
        }

        private async Task<IEnumerable<FornecedorViewModel>> ObterTodosFornecedores()
        {
            var fornecedores = await _fornecedorRepository.ObterTodos();
            return _mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);
        }
    }
}
