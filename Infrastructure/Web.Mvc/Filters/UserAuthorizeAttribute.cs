using Domain;
using Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Web.Framework;
using Web.Jwt;

namespace Web.Mvc
{
    public class UserAuthorizeAttribute  : ActionFilterAttribute
    {
         ILogger logger;

        //不能打开构造函数，否则全局注入UserAuthorizeAttribute后，会抛异常
        //public UserAuthorizeAttribute(bool isLogin)
        //{
        //    IsLogin = isLogin;
        //}

        /// <summary>
        /// 是否登录检查标识
        /// </summary>
        public bool IsLogin { get; set; }

        /// <summary>
        /// 鉴权
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>().CreateLogger(typeof(UserAuthorizeAttribute));
            var configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
            var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();

            var jwtToken = context.HttpContext.RequestServices.GetService(typeof(IJwtToken)) as IJwtToken;
            CurrentUser user = null;
            //var access_token = string.Empty;
            if (authorization.IsEmpty())
            {
                logger.LogInformation($"Headers中的Authorization为空了");
                authorization = context.HttpContext.Request.Query["para2"].ToString() ?? "";

                if (authorization.IsEmpty()) authorization = context.HttpContext.Request.Cookies["access_token"] ?? "";
                if (authorization.IsEmpty())
                {
                    //logger.LogInformation($"Cookies中的access_token为空了");
                    string url = configuration["BuyDongWeb"] + "/api/Account/CreateTempToken";
                    logger.LogInformation($"call {url}前");
                    var tokenResult = await RestClientHelper.HttpGetAsync<SystemResult<ClientToken>>(url, null, AuthorizationType.Bearer);
                    authorization = tokenResult.ReturnValue.Token;
                    logger.LogInformation($"call {url}后生成token{authorization}");
                }

                var payload = jwtToken.DecodeJwt(authorization);               
                if (bool.Parse(payload["IsLogin"]))
                {
                    user = await RedisHelper.HGetAsync<CurrentUser>($"{CacheKey.CurrentUser}",payload["Id"]);
                    if (user == null) throw new BLException("token isnot exists");
                    authorization = user.Token;
                }
                context.HttpContext.Request.Headers.Remove("Authorization");
                context.HttpContext.Request.Headers.Add("Authorization", $"Bearer {authorization}");
            }

            //if (await BaseAuthority.CheckTokenAuthorize(context, next, IsLogin))
            if (await BaseAuthority.CheckMemeberToken(context,next,authorization))
            {              
                var payload = jwtToken.DecodeJwt(authorization);
                var language = payload["Language"];
                var currencyCode = payload["CurrencyCode"];

                //鉴权通过，当前站和buydong的会员token做刷新过期时间              
                if (bool.Parse(payload["IsLogin"]))
                {
                    var result = jwtToken.RefreshToken(authorization, language.ToEnum<Language>(), currencyCode);
                    logger.LogInformation($"call api 刷新token 并更新到redis中,token ={result?.Message ?? ""}");
                    authorization = result.Message;
                }
                
                context.HttpContext.Response.Headers.Remove("Authorization");
                context.HttpContext.Response.Headers.Add("Authorization", $"Bearer {authorization}");

                var option = new CookieOptions { HttpOnly = true };
                context.HttpContext.Response.Cookies.Append("access_token", authorization, option);

                //logger.LogInformation($"设置access_token={access_token}");
                await next();
            }
        }
    }
}
