namespace HKDG.WebSite.Controllers
{
    public class ProductController : BaseMvcController
    {
        IProductCatalogBLL productCatalogBLL;
        IProductBLL productBLL;

        public ProductController(IComponentContext service) : base(service)
        {
            productCatalogBLL = Services.Resolve<IProductCatalogBLL>();
            productBLL = Services.Resolve<IProductBLL>();
        }

        /// <summary>
        /// 类目
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Category(string Id)
        {
            await InitViewPage(Id);

            var result = await productCatalogBLL.GetCatalogAsync();
            result = result.Where(x => x.IspType == ViewBag.IspType).ToList();
            SetTempData("Category", result);

            return GetActionResult("Category");
        }

        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="Id">ProductCode</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Detail(string Id)
        {
            await InitViewPage(Id);

            var details =await productBLL.GetProductDetailAsync(Id);
            SetTempData("ProudctDetail", details);

            return GetActionResult("Detail");
        }      
    }
}
