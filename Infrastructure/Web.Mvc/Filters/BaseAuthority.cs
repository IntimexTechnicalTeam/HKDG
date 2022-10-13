﻿using Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Framework;
using Web.Jwt;

namespace Web.Mvc
{
    public  class BaseAuthority
    {
        /// <summary>
        /// 检查Token
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <param name="IsLogin">是否登录检查标识</param>
        /// <returns></returns>
        public static async Task<bool> CheckTokenAuthorize(ActionExecutingContext context, ActionExecutionDelegate next,bool IsLogin=false)
        {
            var  jwtToken = context.HttpContext.RequestServices.GetService(typeof(IJwtToken)) as IJwtToken;
            var  Configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

            bool flag = true;
            var attributes = context.ActionDescriptor.FilterDescriptors;
            bool isAnonymous = attributes.Any(p => p.Filter is AllowAnonymousFilter);//匿名标识 无需验证
            if (isAnonymous)
            {
                await next();
                flag = false;
                return flag;
            }

            string token = string.Empty;
            var authorization = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authorization.IsEmpty())
            {
                context.Result = new JsonResult(new SystemResult { Succeeded = false, Message = "缺少Authorization" });
                context.HttpContext.Response.StatusCode = 403;
                flag = false;
                return flag;
            }
            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = authorization.Substring("Bearer ".Length).Trim();
            }

            if (token.IsEmpty())
            {
                context.Result = new JsonResult(new SystemResult { Succeeded = false, Message = "缺少token" });
                context.HttpContext.Response.StatusCode = 403;
                flag = false;
                return flag;
            }

            //#region 沒用，不在這里check,保證這個方法的公用性
            //    string url = $"{Setting.BuyDongWebUrl}/api/account/CheckToken";
            //    var result = await RestClientHelper.HttpGetAsync<SystemResult<string>>(url, token, AuthorizationType.Bearer);
            //    if (!result.Succeeded)
            //    {
            //        context.Result = new JsonResult(new SystemResult { Succeeded = false, Message = result.Message });
            //        context.HttpContext.Response.StatusCode = 403;
            //        flag = false;
            //        return flag;
            //    }
            //token = result.Message;

            //#endregion

            string userId = "";
            //检查token
            TokenType tokenType = jwtToken.ValidatePlus(token, a => a["iss"] == Configuration["Jwt:Issuer"] && a["aud"] == Configuration["Jwt:Audience"], action => { userId = action["UserId"]; });         
            if (tokenType == TokenType.Fail)
            {
                context.Result = new JsonResult(new SystemResult { Succeeded = false, Message = "token验证失败" });
                context.HttpContext.Response.StatusCode = 403;
                flag = false;
                return flag;
            }
            if (tokenType == TokenType.Expired)
            {
                context.Result = new JsonResult(new SystemResult { Succeeded = false, Message = "token过期" });
                context.HttpContext.Response.StatusCode = 401;
                flag = false;
                return flag;
            }

            var payload = jwtToken.DecodeJwt(token);
            if (payload["LoginType"].ToString() == LoginType.TempUser.ToString())      //临时用户也是匿名处理
            {
                await next();
                flag = false;
                return flag;
            }

            //检查用户是否登录
            ///if (IsLogin)
            {
                if (!bool.Parse(payload["IsLogin"]))
                {
                    context.Result = new JsonResult(new SystemResult { Succeeded = false, Message = "请登录" });
                    context.HttpContext.Response.StatusCode = 401;
                    flag = false;
                    return flag;
                }
            }

            return flag;
        }
    }
}
