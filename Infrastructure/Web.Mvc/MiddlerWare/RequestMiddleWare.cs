using Castle.Core.Internal;
using Domain;
using Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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
          
            var ispType = context.Request.Query["IspType"];
            var access_token = context.Request.Query["access_token"];

            ////暂时不处理这里
            //if (!ispType.IsNullOrEmpty())
            //{               
            //    if (ispType != Globals.Configuration["IspType"])
            //    {
            //        var returnUrl = context.Request.GetDisplayUrl().Replace($"{ispType}", _config["IspType"])
            //                        .Replace("&para2=", "").Replace(access_token, "");
            //        context.Response.Redirect(returnUrl);
            //        return;                  
            //    }
            //}

            if (!access_token.IsNullOrEmpty())
            {
                context.SetCookie("access_token", access_token);
                
                //var returnUrl = string.Concat(context.Request.Scheme,"://",context.Request.Host, context.Request.Path);
                var returnUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";

                string param = string.Empty;

                //过滤access_token
                var paramList = context.Request.Query.Where(x => x.Key != "access_token").Select(s => $"{s.Key}={s.Value}");
                if (paramList.Any())  param = string.Join("&", paramList);            
                if (!param.IsEmpty()) returnUrl = $"{returnUrl}?{param}";

                context.Response.Redirect(returnUrl);
                return;     
            }

            await this._next(context);
        }
    }
}
