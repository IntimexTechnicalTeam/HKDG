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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("GetCatProdByPage")]
        [ProducesResponseType(typeof(SystemResult<PageData<ProductSummary>>), 200)]
        public async Task<SystemResult<PageData<ProductSummary>>> GetCatProdByPage([FromForm] CatProdPager pager)
        {
            var result = new SystemResult<PageData<ProductSummary>>();
            result.ReturnValue = await productBLL.GetCatProdPageData(pager);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Search")]
        [ProducesResponseType(typeof(SystemResult<PageData<ProductSummary>>), 200)]
        public async Task<SystemResult<PageData<ProductSummary>>> Search([FromForm] ProductSearchCond condition)
        {
            var result = new SystemResult<PageData<ProductSummary>>();

            var cond = new ProdSearchCond { Key = condition.Key ,PageInfo = condition.PageInfo , ProductSearchType = ProductSearchType.OnSaleProduct };

            result.ReturnValue =await productBLL.SearchFrontProductSummaryAsync(cond);
            //result.ReturnValue  = productBLL.SearchFrontProductSummary(cond);
            result.Succeeded = true;    
            return result;
        }

    }
}
