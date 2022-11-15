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
        public async Task<IActionResult> Category(string IspType)
        {
            await InitViewPage(IspType);

            var result = await productCatalogBLL.GetCatalogAsync();
            result = result.Where(x => x.IspType == ViewBag.IspType).ToList();
            SetViewData("Category", result);

            return GetActionResult("Category");
        }

        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="Id">ProductCode</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Detail(string IspType, string Id)
        {
            await InitViewPage(IspType);

            var details =await productBLL.GetProductDetailAsync(Id);
            SetViewData("ProudctDetail", details);
            
            return GetActionResult("Detail");
        }

        [AllowAnonymous]
        public async Task<ActionResult> Search(string IspType, string key)
        {
            await InitViewPage(IspType);
            if (key.IsEmpty())
                return RedirectToAction("Index", "Default");

            ViewBag.Key = key;         
            return GetActionResult("Search", key);

        }

        [AllowAnonymous]
        public async Task<ActionResult> CatProduct(string IspType, string Id)
        {
            await InitViewPage(IspType);            
            return GetActionResult("CatProduct");

        }


    }
}
