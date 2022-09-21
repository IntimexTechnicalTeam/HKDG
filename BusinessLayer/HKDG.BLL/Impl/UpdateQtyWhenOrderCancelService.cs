namespace HKDG.BLL
{
    /// <summary>
    /// 订单创建后，且在安排发货前，在后台进行取消订单
    /// </summary>
    public class UpdateQtyWhenOrderCancelService : AbstractCalculateQtyService
    {
        IServiceProvider _service;


        public UpdateQtyWhenOrderCancelService(IServiceProvider services) : base(services)
        {
            this._service = services;
        }

        public override async Task<SystemResult> CalcuteQty(Guid SkuId, int Qty, Guid MsgId)
        {           
            string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";
            var InvtReservedQty = await RedisHelper.ZIncrByAsync(InvtReservedQtyKey, SkuId.ToString(), -Qty);

            //更新MsgId 的CalculateQtyQueue表状态
            var result = await base.SendCalculateQty((int)InvtReservedQty, SkuId, MsgId, QtyType.WhenOrderCancel);
            return result;
        }
    }
}
