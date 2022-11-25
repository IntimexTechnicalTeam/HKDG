using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Mvc
{
    public class RequestMiddleWare
    {
        RequestDelegate _next;
        ILogger<RequestMiddleWare> _logger;
        IConfiguration _config;

        public RequestMiddleWare(RequestDelegate next, ILogger<RequestMiddleWare> logger, IConfiguration config)
        {
            this._next = next;
            this._logger = logger;
            this._config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            string bearerToken = context.Request.Headers["Authorization"];
            //if (string.IsNullOrEmpty(bearerToken))
            //{
            //    await this._next(context);
            //    return;
            //}
            var ispType = context.Request.Query["IspType"];
            var access_token = context.Request.Query["para2"];
            if (!ispType.IsNullOrEmpty())
            {               
                //var option = new CookieOptions { HttpOnly = true };
                //context.Response.Cookies.Delete("access_token");
                //context.Response.Cookies.Append("access_token", access_token, option);
                //var returnUrl =  context.Request.GetDisplayUrl().Replace($"{ispType}", _config["IspType"])
                //                .Replace("&para2=","").Replace(access_token,"");

                //context.Response.Redirect(returnUrl);
            }

            await this._next(context);
        }
    }
}
