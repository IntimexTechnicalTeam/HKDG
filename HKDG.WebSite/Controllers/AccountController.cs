using Microsoft.AspNetCore.Mvc;

namespace HKDG.WebSite.Controllers
{
   
    public class AccountController : BaseMvcController
    {
        public AccountController(IComponentContext service) : base(service)
        {
        }

        [AllowAnonymous]
        public IActionResult MyMessage()
        {
            return GetActionResult("MyMessage");
        }      
    }
}
