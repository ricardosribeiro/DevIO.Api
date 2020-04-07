using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevIO.Api.Extensions
{
    public class CustomAuthorize
    {   
        /// <summary>
        /// Verifica se o usuário está autenticado e se a Claim é compatível
        /// </summary>
        /// <param name="context">Uma instância de HttpContext que encapsula as informações sobre o Http Request atual</param>
        /// <param name="claimName">Nome da Claim</param>
        /// <param name="claimValue">Valor da Claim</param>
        /// <returns></returns>
        public static bool ValidarClaimUsuario(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated &&
                context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }
    }

    /// <summary>
    /// Extende a classe TypeFilterAttribute e reescreve seu comportamento.
    /// </summary>
    public class ClaimsAuthorizeAttribute: TypeFilterAttribute
    {

        public ClaimsAuthorizeAttribute(string claimName, string claimValue): base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }
    }

    public class RequisitoClaimFilter : IAuthorizationFilter
    {
        private readonly Claim _claim;

        public RequisitoClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            if(!CustomAuthorize.ValidarClaimUsuario(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
                return; 
            }
        }
    }

}
