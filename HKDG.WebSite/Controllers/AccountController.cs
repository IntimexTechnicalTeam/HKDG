using Castle.Core.Internal;

namespace HKDG.WebSite.Controllers
{

    public class AccountController : BaseMvcController
    {
        IInteractMessageBLL interactMessageBLL;

        public AccountController(IComponentContext service) : base(service)
        {   
            interactMessageBLL = Services.Resolve<IInteractMessageBLL>();
        }

        [AllowAnonymous]
        public async Task<IActionResult> MyMessage(string IspType)
        {
            await InitViewPage(IspType);

            

            return GetActionResult("MyMessage");
        }


        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (CurrentUser.IsLogin && !CurrentUser.IsTempUser)
            {
                return Redirect("/");
            }

            var returnUrl = HttpContext.Request.Query["returnUrl"];
            if (!returnUrl.IsNullOrEmpty() && !returnUrl.Contains("login"))
            {
                ViewBag.ReturnUrl = returnUrl;
            }

            return GetActionResult("Login");
        }

        [AllowAnonymous]
        public async Task<IActionResult> MyCoupon()
        {

            return GetActionResult("MyCoupon");
        }

        [AllowAnonymous]
        public async Task<IActionResult> MemberInfo()
        {

            return GetActionResult("MemberInfo");
        }


    }
}
