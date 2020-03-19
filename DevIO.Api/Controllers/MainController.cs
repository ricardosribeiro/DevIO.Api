using DevIO.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class MainController:ControllerBase
    {

        private readonly INotificador _notificador;

        public MainController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }
        protected ActionResult CustomResponse(object result=null)
        {
            if (OperacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                }); 

            }

            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificaErroModelInvalida(modelState);
            return CustomResponse();
        }
        protected void NotificaErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificaErro(erroMsg);
            }
        }

        protected void NotificaErro(string mensagem)
        {
            _notificador.Handle(new Business.Notificacoes.Notificacao(mensagem));
        }
    }
}
