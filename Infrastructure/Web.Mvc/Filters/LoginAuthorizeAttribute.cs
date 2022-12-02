using Domain;
using Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using Web.Framework;

namespace Web.Mvc
{

    /// <summary>
    /// 登录验证filter
    /// </summary>
    public class LoginAuthorizeAttribute: ActionFilterAttribute
    {
        public bool IsLogin { get; set; }

        public LoginAuthorizeAttribute()
        { 
            
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authorization = context.HttpContext.GetRequestHeader("Authorization");
            if (authorization.IsEmpty())
                authorization = context.HttpContext.Request.Cookies["access_token"] ?? string.Empty;

            authorization = authorization.Replace("Bearer", "").Trim();
            var mUser = await RedisHelper.HGetAsync<CurrentUser>($"{CacheKey.CurrentUser}", authorization);

            if (!mUser.IsLogin)
            {
                throw new LoginException(HKDG.Resources.Message.PleaseLogin,"403");
            }

            await next();
        }
    }
}
