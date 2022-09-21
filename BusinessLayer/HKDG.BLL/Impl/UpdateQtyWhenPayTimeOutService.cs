namespace HKDG.BLL
{
    /// <summary>
    /// 支付超时，支付失败，或支付时手动取消
    /// </summary>
    public class UpdateQtyWhenPayTimeOutService : AbstractCalculateQtyService
    {
        IServiceProvider _service;

        public UpdateQtyWhenPayTimeOutService(IServiceProvider services) : base(services)
        {
            this._service = services;

        }

        public override async Task<SystemResult> CalcuteQty(Guid SkuId, int Qty, Guid MsgId)
        {          
            string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";
            var InvtReservedQty = await RedisHelper.ZIncrByAsync(InvtReservedQtyKey, SkuId.ToString(), -Qty);

            var result = await base.SendCalculateQty((int)InvtReservedQty, SkuId, MsgId, QtyType.WhenPayTimeOut);
            return result;
        }
    }

}
