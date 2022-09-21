namespace HKDG.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributeController : BaseApiController
    {
        public IAttributeBLL attributeBLL;

        public AttributeController(IComponentContext services) : base(services)
        {
            attributeBLL = Services.Resolve<IAttributeBLL>();
        }

        /// <summary>
        /// 获取非库存属性
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        [HttpPost("GetCatalogs")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SystemResult<List<ProdAtt>>), 200)]
        public async Task<SystemResult<List<ProdAtt>>> GetFrontAttribute(ProdAttCond cond)
        {
            var result = new SystemResult<List<ProdAtt>>();
            result.ReturnValue = await attributeBLL.GetFrontAttributeAsync(cond);
            result.Succeeded = true;
            return result;
        }
    }
}
