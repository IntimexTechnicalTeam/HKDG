using HKDG.BLL;
using System.Security.Cryptography;

namespace HKDG.WebSite.Controllers
{
    public class ProductController : BaseMvcController
    {
        IProductCatalogBLL productCatalogBLL;
        IProductBLL productBLL;
        IMerchantBLL merchantBLL;
        public ProductController(IComponentContext service) : base(service)
        {
            productCatalogBLL = Services.Resolve<IProductCatalogBLL>();
            productBLL = Services.Resolve<IProductBLL>();
            merchantBLL = Services.Resolve<IMerchantBLL>();
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
        [ProductViewTrackFilter]
        public async Task<IActionResult> Detail(string IspType, string Id)
        {
            await InitViewPage(IspType);

            var details =await productBLL.GetProductDetailAsync(Id);
            SetViewData("ProudctDetail", details);

            var mchDetail = await merchantBLL.GetMerchantInfoAsync(details.MerchantId);            
            SetViewData("MerchantDetail", mchDetail);

            var relateProdList = productBLL.GetRelatedProduct(details.Id);
            SetViewData("RelateProd", relateProdList);

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
            if (!Guid.TryParse(Id, out var catId)) catId = Guid.Empty;

            ViewBag.CatId = catId;
            await InitViewPage(IspType);
            
            var result = await productCatalogBLL.GetCatalogAsync();
            result = result.Where(x => x.IspType == ViewBag.IspType).ToList();
            SetViewData("Category", result);

            var catalog =await productCatalogBLL.GetCatalogById(catId);
            SetViewData("CurrentCatalog", catalog);

            CatProdPager pager = new CatProdPager() { CatId = catId };                      
            if (!IsMobile)pager.PageSize = 9;
            var catProduct = await productBLL.GetCatProdPageData(pager);
            SetViewData("CatalogProducts", catProduct);

            return GetActionResult("CatProduct");

        }


    }
}
