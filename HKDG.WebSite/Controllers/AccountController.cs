using Castle.Core.Internal;
using Enums;

namespace HKDG.WebSite.Controllers
{
    [Hidden]
    public class AccountController : BaseMvcController
    {
        IInteractMessageBLL interactMessageBLL;

        public AccountController(IComponentContext service) : base(service)
        {   
            interactMessageBLL = Services.Resolve<IInteractMessageBLL>();
        }

        [HttpGet("Account/MyMessage/{IspType?}")]
        public async Task<IActionResult> MyMessage(string IspType)
        {
            await InitViewPage(IspType);

            

            return GetActionResult("MyMessage");
        }

        [HttpGet("Account/Login")]
        public async Task<IActionResult> Login()
        {
            await InitViewPage();
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

        [HttpGet("Account/MyCoupon")]
        public async Task<IActionResult> MyCoupon()
        {
            await InitViewPage();
            await InitLastNotice();
            return GetActionResult("MyCoupon");
        }

        [HttpGet("Account/MemberInfo")]
        public async Task<IActionResult> MemberInfo()
        {
            await InitViewPage();
            await InitLastNotice();
            return GetActionResult("MemberInfo");
        }

        [HttpGet("Account/MyFavorite/{IspType?}")]
        public async Task<IActionResult> MyFavorite(string IspType)
        {
            await InitViewPage(IspType);
            return GetActionResult("MyFavorite");
        }
    }
}
