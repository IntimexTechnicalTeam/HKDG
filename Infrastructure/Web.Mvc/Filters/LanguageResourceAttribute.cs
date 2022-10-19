using Intimex.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Web.Jwt;

namespace Web.Mvc
{
    /// <summary>
    /// 资源转换过滤器
    /// </summary>
    public class LanguageResourceAttribute : Attribute, IResultFilter
    {
        public string Lang { get; set; }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            var jwtToken = context.HttpContext.RequestServices.GetService(typeof(IJwtToken)) as IJwtToken;
            var token = context.HttpContext.Request.Cookies["access_token"];
            string langCode = "C";
            if (!token.IsEmpty())
            {
                var payload = jwtToken.DecodeJwt(token);
                langCode = payload["Lang"];
            }
            else
            {
                //暂时先这样，按理要call  buydong sso api
                var tmpToken = jwtToken.CreateDefautToken();
                context.HttpContext.Response.Cookies.Append("access_token", tmpToken);
            }

            //string langCode = ((ViewResult)context.Result).ViewData["Lang"]?.ToString() ?? "C";
            ((ViewResult)context.Result).ViewData["Lang"]=langCode; 
            string cultureName = CultureHelper.GetSupportCulture(langCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
    }
}
