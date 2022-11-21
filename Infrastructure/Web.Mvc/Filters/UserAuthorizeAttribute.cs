using Domain;
using Enums;
using Intimex.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Policy;
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
            if (authorization.IsEmpty())
            {
                logger.LogInformation($"Headers中的Authorization为空了");

                {   //这里是从摆档跳转过的的Token参数
                    authorization = context.HttpContext.Request.Query["para2"].ToString() ?? "";
                    if (!authorization.IsEmpty())
                    {
                        user = await RedisHelper.HGetAsync<CurrentUser>($"{CacheKey.CurrentUser}", authorization);

                        if (user != null && user.ExpireDate >= DateTime.Now) authorization = user?.LoginSerialNO ?? "";
                        else authorization = "";

                    }
                }

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

                context.HttpContext.Request.Headers.Remove("Authorization");
                context.HttpContext.Request.Headers.Add("Authorization", $"Bearer {authorization}");
            }

            var mUser = await RedisHelper.HGetAsync<CurrentUser>($"{CacheKey.CurrentUser}", authorization);

            if (!await BaseAuthority.CheckMemeberToken(context, next, mUser))
            {
                context.HttpContext.Response.Cookies.Delete("access_token");
                context.HttpContext.Response.Redirect("/");
            }

            logger.LogInformation($"{mUser.UserId}鉴权通过");
            var option = new CookieOptions { HttpOnly = true };
            context.HttpContext.Response.Cookies.Append("access_token", authorization, option);

            ////鉴权通过，当前站和buydong的会员token做刷新过期时间
            mUser.ExpireDate = DateTime.Now.AddSeconds(Setting.MemberAccessTokenExpire);
            await RedisHelper.HSetAsync($"{CacheKey.CurrentUser}", authorization, mUser);

            logger.LogInformation($"{mUser.UserId}刷新过期时间");
            await next();

        }
    }
}
