namespace HKDG.WebSite.Controllers
{
    public class TranViewController : WebController
    {
        public TranViewController(IComponentContext service) : base(service)
        {
        }

        public async Task<IActionResult> GoTo(string returnUrl)
        {
            //string returnUrl = HttpContext.Request.GetDisplayUrl().Split("returnUrl=")[1] ?? "";

            if (string.IsNullOrEmpty(returnUrl))
            {
                Response.Redirect("/Default/Index");
                return View();
            }
            
            if (returnUrl.IndexOf("?") >-1) returnUrl += $"&access_token={CurrentUser.LoginSerialNO}";
            else returnUrl += $"?access_token={CurrentUser.LoginSerialNO}";

            var user = await RedisHelper.HGetAsync<CurrentUser>($"{CacheKey.CurrentUser}", CurrentUser.LoginSerialNO);
            user.FromUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.ToString();       //设置跳转源
            await RedisHelper.HSetAsync($"{CacheKey.CurrentUser}", CurrentUser.LoginSerialNO, user);

            HttpContext.Response.Headers.Add("Authorization", $"Bearer {CurrentUser.LoginSerialNO}");           
            HttpContext.Response.Redirect(returnUrl);
            return new EmptyResult();
        }
    }
}