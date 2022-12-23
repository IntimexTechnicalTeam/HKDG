namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCommentController : WebController
    {
        IProductCommentBLL  productCommentBLL;

        public ProductCommentController(IComponentContext service) : base(service)
        {
            productCommentBLL = Services.Resolve<IProductCommentBLL>();
        }

        /// <summary>
        /// 获取产品评论
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [HttpPost("GetProductComments")]
        [ProducesResponseType(typeof(SystemResult<List<ProductCommentDto>>), 200)]
        public async Task<SystemResult<List<ProductCommentDto>>> GetProductComments ([FromForm] SearchCommentsInfo cond)
        {
            var result = new SystemResult<List<ProductCommentDto>>();
            result.ReturnValue = await productCommentBLL.GetProductComments(cond);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 保存产品评论
        /// </summary>
        /// <param name="comments"></param>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpPost("SaveComments")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> SaveComments([FromForm] List<ProductCommentDto> comments)
        {
            SystemResult result = await productCommentBLL.SaveComments(comments);
            return result;
        }

        [HttpGet("GetSubOrderComments")]
        [LoginAuthorize]
        [ProducesResponseType(typeof(SystemResult<List<ProductCommentDto>>), 200)]
        public async Task<SystemResult<List<ProductCommentDto>>> GetSubOrderComments(Guid subOrderId)
        {
            var result = new SystemResult<List<ProductCommentDto>>();
            result.ReturnValue =await productCommentBLL.GetSubOrderComments(subOrderId);
            result.Succeeded = true;
            return result;
        }
    }
}
