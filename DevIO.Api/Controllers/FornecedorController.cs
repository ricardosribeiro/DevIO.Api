using AutoMapper;
using DevIO.Api.Extensions;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Authorize]
    public class FornecedorController : MainController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;
        private readonly IUser _appUser;

        public FornecedorController(
            IFornecedorRepository fornecedorRepository,
            IEnderecoRepository enderecoRepository,
            IFornecedorService fornecedorService,
            IMapper mapper,
            INotificador notificador,
            IUser appUser) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
            _appUser = appUser;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterTodos()
        {
            var fornecedoresViewModel = await ObterTodosFornecedores();

            if (fornecedoresViewModel == null) return NotFound();

            return Ok(fornecedoresViewModel);

        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorPorId(id);
            if (fornecedorViewModel == null) return NotFound();

            return Ok(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor","Adicionar")]
        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar([FromBody] FornecedorViewModel fornecedorViewModel)
        {
            var UserName = _appUser.Name;
            var UserEmail = _appUser.GetUserId();
            var Autenticado = _appUser.IsAuthenticated();
            var isInRole = _appUser.IsInRole("Fornecedor");
            var Claims = _appUser.GetClaimsIdentity();


            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));

            return CustomResponse(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Atualizar")]
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

        [ClaimsAuthorize("Fornecedor", "Remover")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remover(Guid id)
        {
            var fornecedorViewModel = _fornecedorRepository.ObterPorId(id);

            if (fornecedorViewModel == null) return NotFound();

            await _fornecedorService.Remover(id);

            return CustomResponse();
        }

        [AllowAnonymous]
        [HttpGet("obter-endereco/{id:guid}")]
        public async Task<ActionResult>ObterEnderecoPorId(Guid id)
        {
            EnderecoViewModel enderecoViewModel = await ObterEndereco(id);

            if (enderecoViewModel == null) return NotFound();

            return CustomResponse(enderecoViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Atualizar")]
        [HttpPut("atualizar-endereco/{id:guid}")]
        public async Task<ActionResult> AtualizarEndereco(Guid id, [FromBody] EnderecoViewModel enderecoViewModel)
        {
            if (id != enderecoViewModel.Id)
            {
                NotificaErro("O Id informado não corresponde ao endereço.");
                CustomResponse();
            }

            if (!ModelState.IsValid) return CustomResponse();

            var endereco = _mapper.Map<Endereco>(enderecoViewModel);
            await _enderecoRepository.Atualizar(endereco);

            return CustomResponse(enderecoViewModel);

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
        private async Task<EnderecoViewModel> ObterEndereco(Guid id)
        {
            return _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));
        }

        #endregion
    }
}
