using Microsoft.AspNetCore.Http.Extensions;

namespace HKDG.WebSite.Controllers
{
    public class TranViewController : BaseMvcController
    {
        public TranViewController(IComponentContext service) : base(service)
        {
        }

        
        public async Task<ActionResult> GoTo()
        {
            string returnUrl = HttpContext.Request.GetDisplayUrl().Split("returnUrl=")[1] ?? "";

            if (string.IsNullOrEmpty(returnUrl))
            {
                Response.Redirect("/Default/Index");
                return View();
            }
            
            if (returnUrl.IndexOf("?") >-1) returnUrl += $"&access_token={CurrentUser.LoginSerialNO}";
            else returnUrl += $"?access_token={CurrentUser.LoginSerialNO}";

            HttpContext.Response.Headers.Add("Authorization", $"Bearer {CurrentUser.LoginSerialNO}");
            HttpContext.Response.Redirect(returnUrl);
            return new EmptyResult();
        }
    }
}