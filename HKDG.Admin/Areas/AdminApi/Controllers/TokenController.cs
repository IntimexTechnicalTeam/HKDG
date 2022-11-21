using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Framework;


namespace HKDG.Admin.Areas.AdminApi.Controllers
{
    [Area("AdminApi")]
    [Route("adminapi/[controller]/[action]")]
    [ApiController]
    public class TokenController : Controller
    {
        [AllowAnonymous]
        public async Task<SystemResult> Check()
        {
            var result = new SystemResult() { Succeeded = false };
            var access_token = Request.Cookies[StoreConst.AccessToken];
            
            var user =await RedisHelper.HGetAsync<CurrentUser>($"{CacheKey.CurrentUser}", access_token);
            if (user != null)
            {
                if (user.ExpireDate < DateTime.Now) return result;

                result.Message = DateTime.Now.ToShortDateString();
                result.Succeeded = true;
            }

            return result;
        }
    }
}
