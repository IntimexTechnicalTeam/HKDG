using Intimex.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace HKDG.WebSite
{
    /// <summary>
    ///  
    /// </summary>
    public class LanguageFilter : ActionFilterAttribute, IResultFilter
    {


        //public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        //{
        //    var token = context.HttpContext.Request.Cookies["access_token"];
        //    string langCode = "C";
        //    if (!token.IsEmpty())
        //    {
        //        var user =await RedisHelper.HGetAsync<CurrentUser>($"{CacheKey.CurrentUser}", token);
        //        if (user != null) langCode = user.Language.ToString();
        //    }
        //    if (context.Result is ViewResult)
        //    {

        //        //string langCode = ((ViewResult)context.Result).ViewData["Lang"]?.ToString() ?? "C";
        //        //((ViewResult)context.Result).ViewData["Lang"] = langCode;
        //        //string cultureName = CultureHelper.GetSupportCulture(langCode);
        //        //Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
        //        //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        //    }
        //}

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var token = context.HttpContext.Request.Cookies["access_token"];
            string langCode = "C";
            if (!token.IsEmpty())
            {
                var user =  RedisHelper.HGet<CurrentUser>($"{CacheKey.CurrentUser}", token);
                if (user != null) langCode = user.Language.ToString();
            }
            if (context.Result is ViewResult)
            {

                //langCode = ((ViewResult)context.Result).ViewData["Lang"]?.ToString() ?? "C";
                //((ViewResult)context.Result).ViewData["Lang"] = langCode;
                //string cultureName = CultureHelper.GetSupportCulture(langCode);
                //Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
                //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }
        }

    }
}
