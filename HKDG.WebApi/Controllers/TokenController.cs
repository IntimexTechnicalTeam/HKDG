namespace HKDG.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseApiController
    {
        IJwtToken jwtToken;
        public TokenController(IComponentContext service) : base(service)
        {
            jwtToken = this.Services.Resolve<IJwtToken>();
        }

        /// <summary>
        /// TempUser访问时调用这个CreateToken
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("CreateToken")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> CreateToken()
        {
            string ticket = jwtToken.CreateDefautToken();
            var result = new SystemResult() { Succeeded = true };
            result.ReturnValue = ticket;
            return result;
        }

        /// <summary>
        /// 当用户接到401状态码时(token过期)调用这个RefreshToken
        /// </summary>
        [AllowAnonymous]
        [HttpGet("RefreshToken")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult<string>> RefreshToken(string Token,Language Lang= Language.C, string CurrencyCode="HKD")
        {
            var result = new SystemResult<string>() { Succeeded = true };
            result= jwtToken.RefreshToken(Token,Lang,CurrencyCode);            
            return result;
        }
    }
}
