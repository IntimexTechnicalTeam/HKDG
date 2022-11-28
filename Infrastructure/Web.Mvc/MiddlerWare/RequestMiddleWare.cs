using Castle.Core.Internal;
using Domain;
using Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Web.Framework;

namespace Web.Mvc
{
    /// <summary>
    /// 一个防参数被恶意篡改中间件
    /// </summary>
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
                if (ispType != Globals.Configuration["IspType"])
                {
                    var returnUrl = context.Request.GetDisplayUrl().Replace($"{ispType}", _config["IspType"])
                                    .Replace("&para2=", "").Replace(access_token, "");
                    context.Response.Redirect(returnUrl);
                    return;                  
                }
            }

            if (!access_token.IsNullOrEmpty())
            {
                var user = await RedisHelper.HGetAsync<CurrentUser>($"{CacheKey.CurrentUser}", access_token);
                if (user == null || user.ExpireDate < DateTime.Now)  //不存在或过期
                {
                    var returnUrl = context.Request.GetDisplayUrl().Replace($"{ispType}", _config["IspType"])
                                    .Replace("&para2=", "").Replace(access_token, "");
                    context.Response.Cookies.Delete("access_token");
                    context.Response.Redirect(returnUrl);
                    return;
                }
                //else
                {
                    var option = new CookieOptions { HttpOnly = true, Secure = true };
                    context.Response.Cookies.Append("access_token",access_token, option);
                }
            }

            await this._next(context);
        }
    }
}
