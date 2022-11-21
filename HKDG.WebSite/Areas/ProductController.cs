using HKDG.BLL;
using Intimex.Utility;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Web.Jwt;

namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseApiController
    {
        IProductBLL productBLL;

        public ProductController(IComponentContext service) : base(service)
        {
            productBLL = Services.Resolve<IProductBLL>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="attr1"></param>
        /// <param name="attr2"></param>
        /// <param name="attr3"></param>
        /// <param name="saleTime"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Check")]
        [ProducesResponseType(typeof(SystemResult<ProductCheck>), 200)]
        public async Task<SystemResult<ProductCheck>> Check(string code,Guid attr1,Guid attr2,Guid attr3,DateTime? saleTime)
        {
            var result = await productBLL.CheckSkuStateAsync(code, attr1, attr2, attr3, saleTime?.ToString() ?? "");
            return result;
        }
    }
}
