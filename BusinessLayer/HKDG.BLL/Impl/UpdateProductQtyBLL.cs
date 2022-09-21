namespace HKDG.BLL
{
    /// <summary>
    /// 消费者服务类,用于更新ProductQty表
    /// </summary>
    public class UpdateProductQtyBLL : BaseBLL, IUpdateProductQtyBLL
    {
        public Dictionary<QtyType, Func<TmpProductQty, Task<int>>> dicQtyMethord = new Dictionary<QtyType, Func<TmpProductQty, Task<int>>>();

        IDealProductQtyRepository ProductQtyRepository;

        public UpdateProductQtyBLL(IServiceProvider services) : base(services)
        {         
            ProductQtyRepository = this.Services.GetService(typeof(IDealProductQtyRepository)) as IDealProductQtyRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">PushMessage.Id</param>
        /// <returns></returns>
        public async Task<SystemResult> DealProductQty(Guid Id)
        {
            var result = new SystemResult() { Succeeded = false };
            var msg = await baseRepository.GetModelByIdAsync<PushMessage>(Id);

            if (msg == null)
            {
                result.Message = $"找不到记录{Id}";
                return result;
            }

            if (msg.State == MQState.Deal)
            {
                result.Message = $"记录{Id}已处理";
                return result;
            }

            if (msg.MsgType != MQType.UpdateInvt)
            {
                result.Message = $"记录{Id}的类型不是UpdateInvt";
                return result;
            }

            var model = JsonUtil.ToObject<TmpProductQty>(msg.MsgContent);
           
            if (model.QtyType == QtyType.WhenCreateOrder || model.QtyType == QtyType.WhenPayTimeOut || model.QtyType == QtyType.WhenOrderCancel)
                result = await ProductQtyRepository.UpdateQty(model.InvtReservedQty, model.SkuId, Id);

            if (model.QtyType == QtyType.WhenDeliveryArranged)
                result = await ProductQtyRepository.UpdateQty(model.InvtActualQty, model.SalesQty, model.InvtReservedQty, model.SkuId, model.Id);

            if (model.QtyType == QtyType.WhenReturn || model.QtyType == QtyType.WhenPurchasing)
                result = await ProductQtyRepository.UpdateQty(model.InvtActualQty, model.SalesQty, model.SkuId, Id);

            return result;
        }

        /// <summary>
        /// 补偿回写Qty
        /// </summary>
        /// <returns></returns>
        public async Task<SystemResult> HandleQtyAsync()
        {
            var result = new SystemResult();

            string queue = MQSetting.UpdateQtyQueue;
            string exchange = MQSetting.UpdateQtyExchange;

            var list = await baseRepository.GetListAsync<PushMessage>(x => x.State == MQState.UnDeal && x.QueueName == queue
                          && x.ExchangeName == exchange && x.MsgType == MQType.UpdateInvt);

            var query = list.OrderBy(o => o.CreateDate).Take(100).ToList();
            if (query != null && query.Any())
            {
                //this.Logger.LogInformation($"一共{query.Count()}条");
                foreach (var item in query)
                {
                    //this.Logger.LogInformation($"正在发送MQ消息,队列={queue},消息={item.Id}");
                    rabbitMQService.PublishMsg(queue, exchange, item.Id.ToString());
                }
                //this.Logger.LogInformation($"全部发送完了.....");
                result.Succeeded = true;
            }
            return result;
        }

    }
}
