using HKDG.BLL;
using Intimex.Utility;
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

        [AllowAnonymous]
        [HttpGet("Check")]
        [ProducesResponseType(typeof(SystemResult<ProductCheck>), 200)]
        public async Task<SystemResult<ProductCheck>> Check(string code, Guid attr1, Guid attr2, Guid attr3, string saleTime)
        {
            var result = await productBLL.CheckSkuStateAsync(code, attr1, attr2, attr3, saleTime);
            return result;
        }
    }
}
