using Microsoft.AspNetCore.Mvc.Filters;

namespace HKDG.WebApi
{
    /// <summary>
    /// 产品浏览轨迹Filter
    /// </summary>
    public class ProductViewTrackFilter : ActionFilterAttribute, IActionFilter
    {


        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var productBLL = context.HttpContext.RequestServices.GetService(typeof(IProductBLL)) as IProductBLL;
            var result = ((SystemResult<MicroProductDetail>)((ObjectResult)context.Result).Value).ReturnValue;
            var code = result.Code;
            await productBLL.CountClick(code, false);
        }
    }
}
