namespace HKDG.BLL
{
    public class UpdateQtyWhenDeleteCartService : AbstractCalculateQtyService
    {
        IServiceProvider _service;
        public UpdateQtyWhenDeleteCartService(IServiceProvider services) : base(services)
        {
            this._service = services;
        }

        public override async Task<SystemResult> CalcuteQty(Guid SkuId, int Qty, Guid MsgId)
        {
            string SalesQtyKey = $"{CacheKey.SalesQty}";
            string InvtHoldQtyKey = $"{CacheKey.InvtHoldQty}";
            string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
            string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";

            var InvtActualQty = await RedisHelper.ZScoreAsync(InvtActualQtyKey, SkuId) ?? 0;
            if (InvtActualQty < 0) InvtActualQty = 0;

            var InvtReservedQty = await RedisHelper.ZScoreAsync(InvtReservedQtyKey, SkuId) ?? 0;
            if (InvtReservedQty < 0) InvtReservedQty = 0;

            //获取Hold货数
            var InvtHoldQty = await RedisHelper.ZScoreAsync(InvtHoldQtyKey, SkuId) ?? 0;
            //计算Hold货数
            InvtHoldQty = InvtHoldQty - Qty < 0 ? 0 : InvtHoldQty - Qty;
            //设置Hold货数
            await RedisHelper.ZAddAsync(InvtHoldQtyKey, (InvtHoldQty, SkuId.ToString()));

            //设置可销售库存数
            var SalesQty = InvtActualQty - InvtReservedQty - InvtHoldQty;
            if (SalesQty < 0) SalesQty = 0;

            await RedisHelper.ZAddAsync(SalesQtyKey, (SalesQty, SkuId.ToString()));

            //更新MsgId 的CalculateQtyQueue表状态
            var result = await base.SendCalculateQty(InvtActualQty, InvtHoldQty, SalesQty, InvtReservedQty, SkuId, MsgId, QtyType.WhenDeleteCart);
            return result;
        }
    }
}
