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
            SetTempData("LatesNotice", data);

            return GetActionResult("MyMessage");
        }
    }
}
