using Castle.Core.Internal;
using Enums;

namespace HKDG.WebSite.Controllers
{

    public class AccountController : BaseMvcController
    {
        IInteractMessageBLL interactMessageBLL;

        public AccountController(IComponentContext service) : base(service)
        {   
            interactMessageBLL = Services.Resolve<IInteractMessageBLL>();
        }

        public async Task<IActionResult> MyMessage(string IspType)
        {
            await InitViewPage(IspType);

            

            return GetActionResult("MyMessage");
        }

        public async Task<IActionResult> Login()
        {
            await InitLastNotice();
            if (CurrentUser.IsLogin && !CurrentUser.IsTempUser)
            {
                return Redirect("/");
            }

            var returnUrl = HttpContext.Request.QueryString.Value ?? "";
            if (!returnUrl.IsNullOrEmpty() && !returnUrl.Contains("login"))
            {
                returnUrl = returnUrl.Split("returnUrl=")?[1] ?? "";
                ViewBag.ReturnUrl = returnUrl;
            }

            return GetActionResult("Login");
        }

        public async Task<IActionResult> MyCoupon()
        {
            await InitLastNotice();
            return GetActionResult("MyCoupon");
        }

        public async Task<IActionResult> MemberInfo()
        {
            await InitLastNotice();
            return GetActionResult("MemberInfo");
        }

        public async Task<IActionResult> MyFavorite(string IspType)
        {
            await InitViewPage(IspType);
            return GetActionResult("MyFavorite");
        }
    }
}
