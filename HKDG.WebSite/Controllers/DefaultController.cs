using HKDG.BLL;

namespace HKDG.WebSite.Controllers
{
    [Hidden]
    public class DefaultController : WebController
    {
        IIspProviderBLL ispProviderBLL;
        IPromotionBLL promotionBLL;
        IProductCatalogBLL productCatalogBLL;
        ISettingBLL settingBLL;
        IProductBLL productBLL;
        public DefaultController(IComponentContext service) : base(service)
        {
            ispProviderBLL = Services.Resolve<IIspProviderBLL>();
            promotionBLL = Services.Resolve<IPromotionBLL>();
            productCatalogBLL = Services.Resolve<IProductCatalogBLL>();
            settingBLL = Services.Resolve<ISettingBLL>();
             productBLL = Services.Resolve<IProductBLL>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">IspType</param>
        /// <returns></returns>      
        //[HttpGet("Default/Index/{IspType?}")]  //这里不用设置，在EndPointsFactory已做终结点绑定
        public async Task<IActionResult> Index(string IspType)
        {
            await InitViewPage(IspType);

            var cond = new PromotionCond { 
                IspType = ViewBag.IspType, ShowBanner = true, ShowProduct = true, ShowMerchant = false,
                From = !IsMobile ?  ClientSideType.Desktop : ClientSideType.Mobile,PageStr="home"
            };
     
            var promotionList = await promotionBLL.ShowPromotionList(cond);
            SetViewData("PromotionList", promotionList);
            SetViewData("User", CurrentUser);

            return GetActionResult("Index");
        }

        [HttpGet("Default/Menu/{IspType?}")]
        public async Task<IActionResult> Menu(string IspType)
        {
            await InitViewPage(IspType);

            //var cond = new PromotionCond { IspType = IspType, ShowBanner = true, ShowProduct = true, ShowMerchant = false };
            //var promotionList = await promotionBLL.ShowPromotionList(cond);
            //SetViewData("PromotionList", promotionList);

            return GetActionResult("Menu");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("Default/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        public async Task<PartialViewResult> Meta() {

            if (HttpContext.Request.Path.ToString().IndexOf("product/detail") > -1)
            {
                var Id = HttpContext.Request.RouteValues["Id"]?.ToString() ?? "";

                if (Id.IsEmpty()) { throw new BLException("缺少参数"); }

                var product = productBLL.GetProductSeo(Id);
                SetTempData("ProdTitle", product[1]);
                SetTempData("Description", product[2]);
                SetTempData("Keywords", product[3]);
                SetTempData("FacebookImage", this.Configuration["BuyDongWeb"] + product[4]);
                SetTempData("FacebookdDescription", product[5]);
                SetTempData("Url", this.Configuration["BuyDongWeb"] + "/product/detail?id=" + Id);
                SetTempData("Title", product[1]);
            }           
            else
            {
                string key = CacheKey.System.ToString();
                string field = $"{CacheField.Info}_{CurrentUser.Lang}";

                var system = await RedisHelper.HGetAsync<SystemInfoDto>(key, field);
                if (system == null)
                {
                    system = settingBLL.GetSystemInfo(CurrentUser.Lang);
                    await RedisHelper.HSetAsync(key, field, system);
                }
                var mallConfig = system.mallConfig;

                SetTempData("Description", mallConfig.Description);
                SetTempData("Keywords", mallConfig.Keywords);
                SetTempData("FacebookImage", mallConfig.Image);
                SetTempData("FacebookdDescription", mallConfig.Description);
                SetTempData("Url", this.Configuration["BuyDongWeb"]);
                SetTempData("Title", mallConfig.MallName);
            }

            if (IsMobile)
            {
                return PartialView("~/Views/Shared/MobilePartialMeta.cshtml");
            }
            else
            {
                return PartialView("~/Views/Shared/PartialMeta.cshtml");
            }
        }
    }
}