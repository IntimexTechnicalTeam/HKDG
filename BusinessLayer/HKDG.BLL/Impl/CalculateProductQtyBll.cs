namespace HKDG.BLL
{
    /// <summary>
    /// 消费者服务类
    /// </summary>
    public class CalculateProductQtyBll : BaseBLL, ICalculateProductQtyBll
    {
        IServiceProvider _service;

        IDealProductQtyRepository ProductQtyRepository;

        IComponentContext _componentContext;

        public Dictionary<QtyType, ICalculateQtyService> dicService = new Dictionary<QtyType, ICalculateQtyService>();

        public CalculateProductQtyBll(IServiceProvider services, IComponentContext componentContext) : base(services)
        {
            this._service = services;
            this._componentContext = componentContext;
            ProductQtyRepository = this.Services.GetService(typeof(IDealProductQtyRepository)) as IDealProductQtyRepository;
            InitDictService();
        }

        /// <summary>
        /// 消费者计算Qty
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<SystemResult> CalculateQty(Guid Id)
        {
            var result = new SystemResult() { Succeeded = false };
            var msg = await baseRepository.GetModelByIdAsync<CalculateQtyQueue>(Id);

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

            var service = dicService[msg.QtyType];
            var model = new CalculateQty { Id = Id, Qty = msg.Qty, SkuId = msg.SkuId, QtyType = msg.QtyType, RelatedId = msg.RelatedId };
            result = await service.CalcuteQty(model.SkuId, model.Qty, model.Id);
            return result;
        }

        void InitDictService()
        {
            var PurchasingService = _componentContext.ResolveNamed<ICalculateQtyService>(typeof(UpdateQtyWhenPurchasingService).Name);
            var PurchaseReturnService = _componentContext.ResolveNamed<ICalculateQtyService>(typeof(UpdateQtyWhenPurchaseReturnService).Name);
            var ReturnService = _componentContext.ResolveNamed<ICalculateQtyService>(typeof(UpdateQtyWhenReturnService).Name);
            //var AddToCartService = _componentContext.ResolveNamed<ICalculateQtyService>(typeof(UpdateQtyWhenAddToCartService).Name);
            //var DeleteCartService = _componentContext.ResolveNamed<ICalculateQtyService>(typeof(UpdateQtyWhenDeleteCartService).Name);
            //var ModifyCartService = _componentContext.ResolveNamed<ICalculateQtyService>(typeof(UpdateQtyWhenModifyCartService).Name);
            // var PayService = _componentContext.ResolveNamed<ICalculateQtyService>(typeof(UpdateQtyWhenPayconfirmService).Name);
            var DeliveryArrangedService = _componentContext.ResolveNamed<ICalculateQtyService>(typeof(UpdateQtyWhenDeliveryArrangedService).Name);
            var OrderCancelService = _componentContext.ResolveNamed<ICalculateQtyService>(typeof(UpdateQtyWhenOrderCancelService).Name);
            var PayTimeOutService = _componentContext.ResolveNamed<ICalculateQtyService>(typeof(UpdateQtyWhenPayTimeOutService).Name);
            var CreateOrderSerivce = _componentContext.ResolveNamed<ICalculateQtyService>(typeof(UpdateQtyWhenCreateOrderService).Name);

            dicService.Add(QtyType.WhenPurchasing, PurchasingService);
            dicService.Add(QtyType.WhenPurchasingReturn, PurchaseReturnService);
            dicService.Add(QtyType.WhenReturn, ReturnService);
            //dicService.Add(QtyType.WhenAddToCart, AddToCartService);
            //dicService.Add(QtyType.WhenDeleteCart, DeleteCartService);
            //dicService.Add(QtyType.WhenModifyCart, ModifyCartService);
            //dicService.Add(QtyType.WhenPay, PayService);
            dicService.Add(QtyType.WhenDeliveryArranged, DeliveryArrangedService);
            dicService.Add(QtyType.WhenOrderCancel, OrderCancelService);
            dicService.Add(QtyType.WhenPayTimeOut, PayTimeOutService);

            dicService.Add(QtyType.WhenCreateOrder, CreateOrderSerivce);

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
                this.Logger.LogInformation($"一共{query.Count()}条");
                foreach (var item in query)
                {
                    this.Logger.LogInformation($"正在发送MQ消息,队列={queue},消息={item.Id}");
                    rabbitMQService.PublishMsg(queue, exchange, item.Id.ToString());
                }
                this.Logger.LogInformation($"全部发送完了.....");
                result.Succeeded = true;
            }
            return result;
        }
    }
}
