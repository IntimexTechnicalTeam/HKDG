using HKDG.BLL;
using WebCache;

namespace HKDG.WebSite.Controllers
{
    public class DefaultController : BaseMvcController
    {
        IIspProviderBLL ispProviderBLL;
        IPromotionBLL promotionBLL;
        IProductCatalogBLL productCatalogBLL;

        public DefaultController(IComponentContext service) : base(service)
        {
            ispProviderBLL = Services.Resolve<IIspProviderBLL>();
            promotionBLL = Services.Resolve<IPromotionBLL>();
            productCatalogBLL = Services.Resolve<IProductCatalogBLL>();
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="Id">IspType</param>
       /// <returns></returns>      
        public async Task<IActionResult> Index(string IspType)
        {
            await InitViewPage(IspType);

            var cond = new PromotionCond { IspType = IspType, ShowBanner = true, ShowProduct = true, ShowMerchant = false };
            var promotionList = await promotionBLL.ShowPromotionList(cond);
            SetViewData("PromotionList", promotionList);

            var result = await productCatalogBLL.GetCatalogAsync();
            result = result.Where(x => x.IspType == ViewBag.IspType).ToList();
            SetViewData("Category", result);

            return GetActionResult("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}