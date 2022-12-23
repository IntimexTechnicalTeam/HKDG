namespace HKDG.Admin
{
    public class WebController : BaseController
    {
        ICustomMenuBLL customMenuBLL;
        IIspProviderBLL ispProviderBLL;
        IInteractMessageBLL interactMessageBLL;
        IProductCatalogBLL productCatalogBLL;
        ILoginBLL loginBLL;

        public WebController(IComponentContext service) : base(service)
        {
            ispProviderBLL = Services.Resolve<IIspProviderBLL>();
            customMenuBLL = Services.Resolve<ICustomMenuBLL>();
            interactMessageBLL = Services.Resolve<IInteractMessageBLL>();
            productCatalogBLL = Services.Resolve<IProductCatalogBLL>();
            loginBLL= Services.Resolve<ILoginBLL>();
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

        public override  ActionResult GetActionResult(string viewName)
        {
            ViewBag.IsMobile = IsMobile;
            ViewBag.Lang = CurrentUser.Lang;
            ViewBag.CurrencyCode = CurrentUser.CurrencyCode;
            ViewBag.BuyDong = Configuration["BuyDongWeb"];

           return base.GetActionResult(viewName);
        }

        public bool HasPermission(string funcName)
        {
            try
            {
                if (!CurrentUser.Roles.Any()) return false;

                foreach (var role in CurrentUser.Roles)
                {
                    foreach (var p in role.PermissionList)
                    {
                        if (p.Function == funcName)
                        {
                            return true;
                        }
                    }

                }
                return false;
            }
            catch (Exception)
            {
                return false;
                //throw;
            }
        }
    }
}
