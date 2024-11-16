using AppWebSpa.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppWebSpa.Core.Attributes
{
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute(string permission, string module) : base(typeof(CustomAuthorizeFilter))
        {
            Arguments = [permission, module];
        }
    }

    internal class CustomAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly string _permission;
        private readonly string _module;
        private readonly IUsersService _usersService;

        public CustomAuthorizeFilter(string permission, string module, IUsersService usersService)
        {
            _permission = permission;
            _module = module;
            _usersService = usersService;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool isAuthorized = await _usersService.CurrentUserIsAuthorizedAsync(_permission, _module);
            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
