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
        public async Task<IActionResult> MyMessage(string Id)
        {
            await InitViewPage(Id);

            var data = await interactMessageBLL.GetLatesNoticeAsync();
            SetTempData("LatesNotice", data);

            return GetActionResult("MyMessage");
        }
    }
}
