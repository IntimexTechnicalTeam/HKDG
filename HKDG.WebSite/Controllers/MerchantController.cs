namespace HKDG.WebSite.Controllers
{
    [Hidden]
    public class MerchantController :   WebController
    {
        IMerchantBLL merchantBLL;

        public MerchantController(IComponentContext service) : base(service)
        {
            merchantBLL = Services.Resolve<IMerchantBLL>();
        }

        [AllowAnonymous]
        [HttpGet("Merchant/List/{key}/{IspType?}")]
        public async Task<IActionResult> List(string key, string IspType)
        {
            await InitViewPage(IspType);

            ViewBag.SearchKey = key;
            var cond = new MerchantCond { Name = key , Page = 1, PageSize = 15 };
            var result = await merchantBLL.GetMchListAsync(cond);
            SetViewData("Merchants", result);
            return GetActionResult("List");
        }

        /// <summary>
        /// 商家详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Merchant/Detail/{Id}/{IspType?}")]
        public async Task<ActionResult> Detail(Guid Id, string IspType)
        {
            await InitViewPage(IspType);
            var mchDetail = await merchantBLL.GetMerchantInfoAsync(Id);
            var mchProductList = await GetMchProduct(Id);

            SetViewData("MerchantDetail", mchDetail);
            SetViewData("MerchantProdData", mchProductList);

            return GetActionResult("Detail");
        }

        /// <summary>
        /// 获取商家所属的商品分页列表
        /// </summary>
        /// <param name="MchId"></param>
        /// <returns></returns>
        async Task<PageData<ProductSummary>> GetMchProduct(Guid MchId)
        {
            var cond = new ProductCond { MerchantId = MchId , Page =1 ,PageSize =12, OrderBy="New" };
            var result = await merchantBLL.GetMchProductListAsync(cond);
            return result;
        }

    }
}
