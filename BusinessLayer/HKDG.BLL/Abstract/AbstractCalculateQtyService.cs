using HKDG.Repository;

namespace HKDG.BLL
{
    public abstract class AbstractCalculateQtyService : BaseBLL, ICalculateQtyService
    {
        IServiceProvider _service;
        public IRabbitMQService _rabbitMQService { get; set; }

        public ICalculateQtyQueueRepository CalculateQtyQueueRepository => this.Services.GetService(typeof(ICalculateQtyQueueRepository)) as ICalculateQtyQueueRepository;

        protected AbstractCalculateQtyService(IServiceProvider services) : base(services)
        {
            this._service = services;
            this._rabbitMQService = base.rabbitMQService;  //这里要重新注入一下
        }

        public abstract Task<SystemResult> CalcuteQty(Guid SkuId, int Qty, Guid MsgId);

        /// <summary>
        /// 创建订单,支付前取消，支付后到發貨前取消，支付超時或失败时，更新 InvtReservedQty
        /// </summary>
        /// <param name="InvtReservedQty"></param>
        /// <param name="SkuId"></param>
        /// <param name="MsgId"></param>
        /// <param name="QtyType"></param>
        /// <returns></returns>
        public async Task<SystemResult> SendCalculateQty(int InvtReservedQty, Guid SkuId, Guid MsgId, QtyType QtyType)
        {
            var msg = GenMsg(0, 0, 0, InvtReservedQty, SkuId, QtyType);
            var result = await SendQtyMsg(msg, MsgId);
            return result;
        }

        /// <summary>
        /// 采購入庫,采購退回,銷售退回，更新 InvtActualQty，SalesQty
        /// </summary>
        /// <param name="InvtActualQty"></param>
        /// <param name="SalesQty"></param>
        /// <param name="SkuId"></param>
        /// <param name="MsgId"></param>
        /// <param name="QtyType"></param>
        /// <returns></returns>
        public async Task<SystemResult> SendCalculateQty(int InvtActualQty, int SalesQty, Guid SkuId, Guid MsgId, QtyType QtyType)
        {
            var msg = GenMsg(InvtActualQty, 0, SalesQty, 0, SkuId, QtyType);
            var result = await SendQtyMsg(msg, MsgId);
            return result;
        }

        /// <summary>
        /// 安排发货时
        /// </summary>
        /// <param name="InvtReservedQty"></param>
        /// <param name="InvtActualQty"></param>
        /// <param name="SaleQty"></param>
        /// <param name="SkuId"></param>
        /// <param name="MsgId"></param>
        /// <param name="QtyType"></param>
        /// <returns></returns>
        public async Task<SystemResult> SendCalculateQty(int InvtReservedQty, int InvtActualQty, int SaleQty, Guid SkuId, Guid MsgId, QtyType QtyType)
        {
            var msg = GenMsg(InvtActualQty, 0, SaleQty, InvtReservedQty, SkuId, QtyType);
            var result = await SendQtyMsg(msg, MsgId);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InvtActualQty"></param>
        /// <param name="InvtHoldQty">没用参数</param>
        /// <param name="SalesQty"></param>
        /// <param name="InvtReservedQty"></param>
        /// <param name="SkuId"></param>
        /// <param name="QtyType"></param>
        /// <returns></returns>
        PushMessage GenMsg(int InvtActualQty, int InvtHoldQty, int SalesQty, int InvtReservedQty, Guid SkuId, QtyType QtyType)
        {
            var productQty = new TmpProductQty
            {
                InvtActualQty = InvtActualQty,
                InvtHoldQty = InvtHoldQty,
                SalesQty = SalesQty,
                InvtReservedQty = InvtReservedQty,
                SkuId = SkuId,
                QtyType = QtyType
            };

            productQty.Id = Guid.NewGuid();
            var msg = new PushMessage
            {
                Id = productQty.Id,
                ExchangeName = MQSetting.UpdateQtyExchange,
                QueueName = MQSetting.UpdateQtyQueue,
                MsgType = MQType.UpdateInvt,
                MsgContent = JsonUtil.ToJson(productQty),
            };

            return msg;
        }

        async Task<SystemResult> SendQtyMsg(PushMessage msg, Guid MsgId)
        {
            var result = new SystemResult();
            var flag = await CalculateQtyQueueRepository.UpdateState(MsgId);
            if (flag > 0) await baseRepository.InsertAsync(msg);

            _rabbitMQService.PublishMsg(msg.QueueName, msg.ExchangeName, msg.Id.ToString());
            result.Succeeded = true;
            return result;
        }

        public async Task<SystemResult> SendCalculateQty(decimal InvtActualQty, decimal InvtHoldQty, decimal SalesQty, decimal InvtReservedQty, Guid SkuId, Guid MsgId, QtyType QtyType)
        {
            var result = new SystemResult();
            var msg = GenMsg((int)InvtActualQty, (int)InvtHoldQty, (int)SalesQty, (int)InvtReservedQty, SkuId, QtyType);
            var flag = await CalculateQtyQueueRepository.UpdateState(MsgId);
            if (flag > 0) await baseRepository.InsertAsync(msg);

            _rabbitMQService.PublishMsg(msg.QueueName, msg.ExchangeName, msg.Id.ToString());
            result.Succeeded = true;
            return result;
        }
    }
}
