using Intimex.Common;
using WS.ECShip.Model.MailTracking;

namespace HKDG.WebSite.Areas
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : WebController
    {
        IOrderBLL orderBLL;
        
        public OrderController(IComponentContext service) : base(service)
        {
            orderBLL = this.Services.Resolve<IOrderBLL>();
        }

        /// <summary>
        /// 创建订单 
        /// </summary>
        /// <param name="checkout"></param>
        /// <returns></returns>
        /// <exception cref="BLException"></exception>
        [LoginAuthorize]
        [HttpPost("Create")]
        [ProducesResponseType(typeof(SystemResult), 200)]
        public async Task<SystemResult> Create([FromForm] NewOrder checkout)
        {
            SystemResult result = new SystemResult();

            if (checkout == null) throw new BLException(HKDG.Resources.Message.SaveFail);

            result = await orderBLL.BuildOrder(checkout);            
            return result;
        }

        /// <summary>
        /// 我的订单 
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpPost("SearchOrder")]
        [ProducesResponseType(typeof(SystemResult<PageData<OrderSummaryView>>), 200)]
        public async Task<SystemResult<PageData<OrderSummaryView>>> SearchOrder([FromForm]OrderPager pager)
        {
            var result = new SystemResult<PageData<OrderSummaryView>>();
            var cond = new OrderCondition
            {
                PageInfo = pager,
                MemberId = CurrentUser.Id,
                StatusCode = pager.Status,
                OrderBy = pager.OrderBy,
                IsFront = true,
            };

            result.ReturnValue = await orderBLL.GetOrders(cond);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 订单明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [LoginAuthorize]
        [HttpGet("GetOrder")]
        [ProducesResponseType(typeof(SystemResult<PageData<OrderSummaryView>>), 200)]
        public async Task <SystemResult<OrderInfoView>> GetOrder(Guid id)
        {
           var result = new SystemResult<OrderInfoView>();
            result.ReturnValue = orderBLL.GetOrder(id);
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 創建退換單
        /// </summary>
        [LoginAuthorize]
        [HttpPost("CreateReturnOrder")]
        [ProducesResponseType(typeof(SystemResult<NewReturnOrder>), 200)]
        public async Task<SystemResult<NewReturnOrder>> CreateReturnOrder([FromBody] NewReturnOrder rOrder)
        {
            foreach (var item in rOrder.Items)
            {
                foreach (var attImg in item.AttachImages)
                {
                    if (!string.IsNullOrEmpty(attImg.ImageSName))
                    {
                        string newImgSName = Guid.NewGuid() + Path.GetExtension(attImg.ImageSName);
                        attImg.NewImageSName = newImgSName;
                    }
                    else
                    {
                        attImg.NewImageSName = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(attImg.ImageBName))
                    {
                        string newImgBName = Guid.NewGuid() + Path.GetExtension(attImg.ImageBName);
                        attImg.NewImageBName = newImgBName;
                    }
                    else
                    {
                        attImg.NewImageBName = string.Empty;
                    }
                }
            }

            var result = await orderBLL.CreateReturnOrder(rOrder);

            Guid rOrderId = result.ReturnValue.Id;
            List<string> files = new List<string>();

            foreach (var item in result.ReturnValue.Items)
            {
                if (item.AttachImages?.Count > 0)
                {                   
                    string merchantId = item.MerchantId.ToString();
                    var tempPhysicalPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], merchantId, FileFolderEnum.TempPath);
                    var targetPhysicalPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"],merchantId, FileFolderEnum.ROrderImage) + rOrderId;
                    //var relPath = PathUtil.GetRelativePath(merchantId, FileFolderEnum.ROrderImage) + "/" + rOrderId;

                    foreach (var attImg in item.AttachImages)
                    {
                        if (!string.IsNullOrEmpty(attImg.ImageSName))
                        {
                            string imgSName = attImg.ImageSName;
                            string imgSNewName = attImg.NewImageSName;
                            var tmpFileSPath = tempPhysicalPath + "\\" + imgSName;
                            Intimex.Utility.FileUtil.MoveFile(tmpFileSPath, targetPhysicalPath, imgSNewName);
                            //files.Add(relPath + "/" + imgSNewName);
                        }

                        if (!string.IsNullOrEmpty(attImg.ImageBName))
                        {
                            string imgBName = attImg.ImageBName;
                            string imgBNewName = attImg.NewImageBName;
                            var tmpFileSPath = tempPhysicalPath + "\\" + imgBName;
                            Intimex.Utility.FileUtil.MoveFile(tmpFileSPath, targetPhysicalPath, imgBNewName);
                            //files.Add(relPath + "/" + imgBNewName);
                        }
                    }
                }
            }


            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 獲取快遞單的物流情況
        /// </summary>
        /// <param name="trackingNo"></param>
        /// <returns></returns>
        [HttpGet("GetOrderMailTrackingInfo")]
        [ProducesResponseType(typeof(SystemResult<MailTrackingInfo>), 200)]
        public async Task<SystemResult<MailTrackingInfo>> GetOrderMailTrackingInfo(string trackingNo)
        {
            var result = new SystemResult<MailTrackingInfo>();
            var data = orderBLL.GetOrderMailTrackingInfo(trackingNo);
            result.ReturnValue = data;
            result.Succeeded = true;
            return result;
        }

        [HttpGet("GetReturnType")]
        [ProducesResponseType(typeof(SystemResult<List<KeyValue>>), 200)]
        public async Task<SystemResult<List<KeyValue>>> GetReturnType()
        {
            var result = new SystemResult<List<KeyValue>>();
            result.ReturnValue = await orderBLL.GetReturnOrderTypeComboSrc();
            result.Succeeded = true;
            return result;
        }
    }
}
