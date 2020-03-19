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
            IMapper mapper,
            INotificador notificador) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterTodos()
        {
            var fornecedoresViewModel = await ObterTodosFornecedores();

            if (fornecedoresViewModel == null) return NotFound();

            return Ok(fornecedoresViewModel);

        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorPorId(id);
            if (fornecedorViewModel == null) return NotFound();

            return Ok(fornecedorViewModel);
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar([FromBody] FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));

            return CustomResponse(fornecedorViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, [FromBody] FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id)
            {
                NotificaErro("O Id informado não corresponde ao Fornecedor");
                CustomResponse();
            }

            if (!ModelState.IsValid) return CustomResponse();

            await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));

            return CustomResponse(fornecedorViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remover(Guid id)
        {
            var fornecedorViewModel = _fornecedorRepository.ObterPorId(id);

            if (fornecedorViewModel == null) return NotFound();

            await _fornecedorService.Remover(id);

            return CustomResponse();
        }

        #region Métodos Auxiliares
        private async Task<FornecedorViewModel> ObterFornecedorPorId(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterPorId(id));
        }

        private async Task<IEnumerable<FornecedorViewModel>> ObterTodosFornecedores()
        {
            var fornecedores = await _fornecedorRepository.ObterTodos();
            return _mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);
        }

        #endregion
    }
}
