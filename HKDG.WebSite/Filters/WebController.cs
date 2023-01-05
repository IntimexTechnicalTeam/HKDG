using Domain;

namespace HKDG.WebSite
{
    public class WebController : BaseController
    {
        ICustomMenuBLL customMenuBLL;
        IIspProviderBLL ispProviderBLL;
        IInteractMessageBLL interactMessageBLL;
        IProductCatalogBLL productCatalogBLL;
        ILoginBLL loginBLL;
        ISettingBLL settingBLL;

        public WebController(IComponentContext service) : base(service)
        {
            ispProviderBLL = Services.Resolve<IIspProviderBLL>();
            customMenuBLL = Services.Resolve<ICustomMenuBLL>();
            interactMessageBLL = Services.Resolve<IInteractMessageBLL>();
            productCatalogBLL = Services.Resolve<IProductCatalogBLL>();
            loginBLL= Services.Resolve<ILoginBLL>();
            settingBLL = Services.Resolve<ISettingBLL>();
        }

        CurrentUser _currentUser;
        public CurrentUser CurrentUser
        {
            get
            {
                string token = CurrentContext?.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Substring("Bearer ".Length).Trim() ?? "";
                if (token.IsEmpty() || token == "undefined") token = CurrentContext?.HttpContext.Request?.Cookies["access_token"]?.ToString() ?? "";

                _currentUser = RedisHelper.HGet<CurrentUser>($"{CacheKey.CurrentUser}", token);

                if (_currentUser == null) _currentUser = new CurrentUser();
                _currentUser.IspType = Configuration["IspType"];
                return _currentUser;
            }
        }

        public override   ActionResult GetActionResult(string viewName)
        {
            ViewBag.IsMobile = IsMobile;
            ViewBag.Lang = CurrentUser.Lang;
            ViewBag.CurrencyCode = CurrentUser.CurrencyCode;
            ViewBag.BuyDong = Configuration["BuyDongWeb"];

           return base.GetActionResult(viewName);
        }

        /// <summary>
        /// 初始化IspType,HeaderMenu,FooterMenu等
        /// </summary>
        /// <param name="IspType"></param>
        /// <returns></returns>
        public virtual async Task InitViewPage(string IspType = "")
        {
            await InitIspType(IspType);
            await InitMenusAsync(ViewBag.IspType);
            await InitLastNotice();
            await InitCategory();         
        }

        public virtual void SetViewData<T>(string key, T t)
        {
            if (t == null) t = default(T);
            var json = JsonUtil.ToJson(t);
            //TempData[key] = json;
            ViewData[key] = json;
        }

        public virtual void SetViewData(string key, string t)
        {
            ViewData[key] = t;
        }

        public virtual void SetTempData<T>(string key, T t) where T : class, new()
        {
            if (t == null) t = default(T);
            var json = JsonUtil.ToJson(t);
            TempData[key] = json;
        }

        public virtual void SetTempData(string key, string value)
        {
            if (value.IsEmpty()) value = default(string);
            TempData[key] = value;
        }

        /// <summary>
        /// 设置ViewBag.IspType
        /// </summary>
        /// <param name="IspType"></param>
        /// <returns></returns>
        /// <exception cref="BLException"></exception>
        async Task InitIspType(string IspType)
        {
            if (IspType.IsEmpty()) IspType = CurrentUser.IspType;
            var flag = await ispProviderBLL.CheckIspType(IspType);
            if (!flag) throw new BLException($"wrong IspType: {IspType}");
            ViewBag.IspType = IspType;
        }

        async Task InitMenusAsync(string IspType)
        {
            var menus = await customMenuBLL.GetMenuBarAsync();
            menus.HeaderMenus = menus.HeaderMenus.Where(x => x.IspType == IspType).ToList();
            menus.FooterMenus = menus.FooterMenus.Where(x => x.IspType == IspType).ToList();

            SetViewData("MenuBarDatas", menus);

        }

        public async Task InitLastNotice()
        {
            var data = await interactMessageBLL.GetLatesNoticeAsync();
            SetViewData("LatesNotice", data);
        }

        public async Task InitCategory()
        {
            var result = await productCatalogBLL.GetCatalogAsync();
            result = result.Where(x => x.IspType == ViewBag.IspType).ToList();
            SetViewData("Category", result);

        }
    }
}
