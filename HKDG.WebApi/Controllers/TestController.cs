namespace HKDG.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseApiController
    {
        public TestController(IComponentContext services) : base(services)
        {
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public string Test()
        {
            string port = HttpContext.Request.HttpContext.Connection.LocalPort.ToString();
            Logger.LogInformation(port);

            return port;
        }
    }
}
