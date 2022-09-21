namespace HKDG.BLL
{
    public class UpdateQtyWhenDeliveryArrangedService : AbstractCalculateQtyService
    {
        IServiceProvider _service;

        public UpdateQtyWhenDeliveryArrangedService(IServiceProvider services) : base(services)
        {
            this._service = services;

        }

        public override async Task<SystemResult> CalcuteQty(Guid SkuId, int Qty, Guid MsgId)
        {
            string SalesQtyKey = $"{CacheKey.SalesQty}";           
            string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
            string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

            //减法
            var InvtActualQty = await RedisHelper.ZIncrByAsync(InvtActualQtyKey, SkuId.ToString(), -Qty);
            var SalesQty = await RedisHelper.ZIncrByAsync(SalesQtyKey, SkuId.ToString(), -Qty);         
            var InvtReservedQty = await RedisHelper.ZIncrByAsync(InvtReservedQtyKey, SkuId.ToString(), -Qty);

            //更新MsgId 的CalculateQtyQueue表状态
            var result = await base.SendCalculateQty((int)InvtReservedQty, (int)InvtActualQty, (int)SalesQty,SkuId, MsgId, QtyType.WhenDeliveryArranged);
            return result;
        }
    }
}
