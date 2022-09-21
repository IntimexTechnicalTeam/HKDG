using HKDG.Domain;

namespace HKDG.BLL
{
    /// <summary>
    /// 处理缓存中的ProductQty
    /// </summary>
    public class DealProductQtyCacheBLL : BaseBLL, IDealProductQtyCacheBLL
    {
        public DealProductQtyCacheBLL(IServiceProvider services) : base(services)
        {
        }

        /// <summary>
        /// 当订单状态改变时，更新库存
        /// </summary>
        /// <param name="orderStatusInfo"></param>
        /// <returns></returns>
        public async Task<SystemResult> UpdateQtyWhenOrderStateChange(UpdateStatusCondition orderStatusInfo)
        {
            var result = new SystemResult();

            var order = await baseRepository.GetModelByIdAsync<Order>(orderStatusInfo.OrderId);
            if (order != null)
            {
                switch (order.Status)
                {
                    case OrderStatus.PaymentConfirmed: result = await UpdateQtyWhenPaymentConfirmed(order.Id); result.ReturnValue = order; break;
                    case OrderStatus.DeliveryArranged: result = await UpdateQtyWhenOrderDeliveryArranged(order.Id); result.ReturnValue = order; break;
                    case OrderStatus.SCancelled: result = await UpdateQtyWhenOrderCancel(order.Id); break;
                    default: result.Succeeded = true; result.ReturnValue = null; break;
                }
            }
            return result;
        }

        /// <summary>
        /// 采购入库,采购退回，銷售退回，發貨退回时更新库存
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<SystemResult> UpdateQtyWhenPurchaseOrReturn(List<InvTransactionDtlDto> list)
        {
            var result = new SystemResult();

            foreach (var item in list)
            {
                switch (item.TransType)
                {
                    case InvTransType.Purchase: result = await UpdateQtyWhenPurchasing(item.Sku, item.TransQty); break;
                    case InvTransType.PurchaseReturn: result = await UpdateQtyWhenPurchaseReturn(item.Sku, item.TransQty); break;
                    case InvTransType.SalesReturn:
                    case InvTransType.DeliveryReturn:
                        result = await UpdateQtyWhenReturn(item.Sku, item.TransQty); break;
                }
            }
            return result;
        }

        /// <summary>
        /// 支付超时，回滚
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public async Task<SystemResult> UpdateQtyWhenPayTimeOut(Guid OrderId)
        {
            var result = new SystemResult();

            var details = await baseRepository.GetListAsync<OrderDetail>(x => x.OrderId == OrderId);
            if (details != null && details.Any())
            {
                var msgList = details.Select(item => new CalculateQtyQueue
                {
                    Qty = item.Qty,
                    SkuId = item.SkuId,
                    QtyType = QtyType.WhenPayTimeOut,
                    Id = Guid.NewGuid(),
                    RelatedId = OrderId
                }).ToList();
                //var kolFlowList = await DeleteCalculateCommission(OrderId);

                using (var tran = baseRepository.CreateTransation())
                {
                    //await baseRepository.UpdateAsync(kolFlowList);
                    await baseRepository.InsertAsync(msgList);
                    tran.Commit();
                }

                foreach (var item in msgList)
                {
                    rabbitMQService.PublishMsg(MQSetting.CalculateQtyQueue, MQSetting.CalculateQtyExchange, item.Id.ToString());
                }
                result.Succeeded = true;
            }

            return result;
        }

        /// <summary>
        /// 采购入库成功，更新库存
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        async Task<SystemResult> UpdateQtyWhenPurchasing(Guid SkuId, int Qty)
        {
            var result = new SystemResult();
            var productQty = new CalculateQtyQueue { Qty = Qty, SkuId = SkuId, QtyType = QtyType.WhenPurchasing, Id = Guid.NewGuid() };

            await baseRepository.InsertAsync(productQty);
            rabbitMQService.PublishMsg(MQSetting.CalculateQtyQueue, MQSetting.CalculateQtyExchange, productQty.Id.ToString());

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 采购退回，更新库存
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty">退回数量</param>
        /// <returns></returns>
        async Task<SystemResult> UpdateQtyWhenPurchaseReturn(Guid SkuId, int Qty)
        {
            var result = new SystemResult();
            var productQty = new CalculateQtyQueue { Qty = Qty, SkuId = SkuId, QtyType = QtyType.WhenPurchasingReturn, Id = Guid.NewGuid() };

            await baseRepository.InsertAsync(productQty);
            rabbitMQService.PublishMsg(MQSetting.CalculateQtyQueue, MQSetting.CalculateQtyExchange, productQty.Id.ToString());
            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 銷售退回或發貨退回时更新库存
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty">退回数量</param>
        /// <returns></returns>
        async Task<SystemResult> UpdateQtyWhenReturn(Guid SkuId, int Qty)
        {
            var result = new SystemResult();
            var productQty = new CalculateQtyQueue { Qty = Qty, SkuId = SkuId, QtyType = QtyType.WhenReturn, Id = Guid.NewGuid() };

            await baseRepository.InsertAsync(productQty);
            rabbitMQService.PublishMsg(MQSetting.CalculateQtyQueue, MQSetting.CalculateQtyExchange, productQty.Id.ToString());

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 当订单状态变更为PaymentConfirmed时
        /// </summary>
        /// <param name="SkuId"></param> 
        /// <param name="Qty"></param>
        /// <returns></returns>
        async Task<SystemResult> UpdateQtyWhenPaymentConfirmed(Guid OrderId)
        {
            var result = new SystemResult();

            //await kolFlowBLL.SendWriteCommisionWhenPayConfirm(OrderId);  //使用MQ处理佣金流水

            result.Succeeded = true;
            return result;
        }

        /// <summary>
        /// 当订单状态变更为DeliveryArranged,已安排发货
        /// </summary>
        /// <param name="SkuId"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>  ---not ok
        async Task<SystemResult> UpdateQtyWhenOrderDeliveryArranged(Guid OrderId)
        {
            var result = new SystemResult();
            var reservedLst = await baseRepository.GetListAsync<InventoryReserved>(x => x.OrderId == OrderId && x.IsActive && !x.IsDeleted
                                               && x.ProcessState == InvReservedState.FINISH);

            if (reservedLst != null && reservedLst.Any())
            {
                var rLst = reservedLst.GroupBy(g => g.Sku).Select(s => new { Sku = s.Key, Qty = s.Sum(x => x.ReservedQty) }).ToList();
                var msgList = rLst.Select(item => new CalculateQtyQueue { Qty = item.Qty, SkuId = item.Sku, QtyType = QtyType.WhenDeliveryArranged, Id = Guid.NewGuid(), RelatedId = OrderId }).ToList();

                await baseRepository.InsertAsync(msgList);
                foreach (var item in msgList)
                {
                    rabbitMQService.PublishMsg(MQSetting.CalculateQtyQueue, MQSetting.CalculateQtyExchange, item.Id.ToString());
                }
                result.Succeeded = true;
            }
            return result;
        }

        /// <summary>
        /// 当订单取消时，回滚预留数
        /// </summary>
        /// <param name="SkuId"></param>       
        /// <param name="Qty"></param>
        /// <returns></returns>  
        async Task<SystemResult> UpdateQtyWhenOrderCancel(Guid OrderId)
        {
            var result = new SystemResult();
            var reservedLst = await baseRepository.GetListAsync<InventoryReserved>(x => x.OrderId == OrderId && x.IsActive && !x.IsDeleted
                                               && x.ProcessState == InvReservedState.CANCEL);

            if (reservedLst != null && reservedLst.Any())               //当订单为已付款，处理中
            {
                var rLst = reservedLst.GroupBy(g => g.Sku).Select(s => new { Sku = s.Key, Qty = s.Sum(x => x.ReservedQty) }).ToList();
                var msgList = rLst.Select(item => new CalculateQtyQueue { Qty = item.Qty, SkuId = item.Sku, QtyType = QtyType.WhenOrderCancel, Id = Guid.NewGuid(), RelatedId = OrderId }).ToList();

                //已付款到按排发货前，后臺取消訂單，如果是Kol訂單的話,回滾流水
                //var rollBackList = await RollBackFlow(OrderId);
                //var kolFlowList = await DeleteCalculateCommission(OrderId);

                using (var tran = baseRepository.CreateTransation())
                {
                    //await baseRepository.UpdateAsync(rollBackList.Item1);
                    //await baseRepository.UpdateAsync(rollBackList.Item2);
                    //await baseRepository.UpdateAsync(kolFlowList);
                    await baseRepository.InsertAsync(msgList);
                    tran.Commit();
                }

                foreach (var item in msgList)
                {
                    rabbitMQService.PublishMsg(MQSetting.CalculateQtyQueue, MQSetting.CalculateQtyExchange, item.Id.ToString());
                }
                result.Succeeded = true;
            }
            else
            {
                //订单是待付款状态（后台可在待付款状态时手动取消订单）或者支付超时或支付失败取消订单
                result = await UpdateQtyWhenPayTimeOut(OrderId);
                result.Succeeded = true;
            }

            return result;
        }

       
    }
}
