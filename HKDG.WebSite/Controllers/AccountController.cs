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

            var data = await interactMessageBLL.GetLatesNoticeAsync();
            SetViewData("LatesNotice", data);

            return GetActionResult("MyMessage");
        }


        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            
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
