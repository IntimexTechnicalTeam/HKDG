using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HKDG.WebSite
{
    /// <summary>
    /// 产品浏览轨迹Filter
    /// </summary>
    public class ProductViewTrackFilter : ActionFilterAttribute, IActionFilter
    {


        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var productBLL = context.HttpContext.RequestServices.GetService(typeof(IProductBLL)) as IProductBLL;

            var flag = context.HttpContext.Request.Query["isSearch"].ToString().ToInt() > 0 ? true : false;

            var code = context.HttpContext.Request.Query["id"];
            await productBLL.CountClick(code, flag);
        }
    }
}
