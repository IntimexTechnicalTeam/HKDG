using Domain;
using Enums;
using Intimex.Runtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
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
            var authorization = context.HttpContext.GetRequestHeader("Authorization");

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
                        else
                        {
                            context.HttpContext.DeleteCookie("access_token");
                            context.HttpContext.Response.Redirect("/");
                        }

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
                    logger.LogInformation($"call {url}后生成token:{authorization}");
                }

                context.HttpContext.SetRequestHeader("Authorization", $"Bearer {authorization}");
            }

            authorization = authorization.Replace("Bearer", "").Trim();
            var flag = await BaseAuthority.CheckMemeberToken(context, next, authorization);
            if (flag == TokenType.Fail)
            {
                await RedisHelper.HDelAsync($"{CacheKey.CurrentUser}", authorization);
                context.HttpContext.DeleteCookie("access_token");
                context.HttpContext.Response.Redirect("/");
                return;
            }

            if (flag == TokenType.Expired) await RefreashToken(context, authorization);  //过期就进行续期

            context.HttpContext.SetCookie("access_token", authorization);
            await next();
        }

        /// <summary>
        /// 刷新会员token过期时间
        /// </summary>
        /// <param name="context"></param>
        /// <param name="authorization"></param>
        /// <returns></returns>
        async Task RefreashToken(ActionExecutingContext context,string authorization)
        {
            var mUser = await RedisHelper.HGetAsync<CurrentUser>($"{CacheKey.CurrentUser}", authorization);           
            mUser.ExpireDate = DateTime.Now.AddSeconds(Setting.MemberAccessTokenExpire);
            mUser.IspType = Globals.Configuration["IspType"];
            await RedisHelper.HSetAsync($"{CacheKey.CurrentUser}", authorization, mUser);

            logger.LogInformation($"{mUser.LoginSerialNO}刷新过期时间");
        }     
    }
}
