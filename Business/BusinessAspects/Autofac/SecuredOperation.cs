using Business.Constants;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Core.Extentions;

namespace Business.BusinessAspects.Autofac
{
    //JWT için
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;

        
        public SecuredOperation(string roles)
        {
            _roles = roles.Split(','); // gelen string i virgüllerden ayırarak array e atar. 
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>(); //Injection altyapısını okumamıza yarayacak
                                                                                                    //ServiceTool

        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                {
                    return;
                }
            }
            throw new Exception(Messages.AuthorizationDenied);
        }
    }
}
