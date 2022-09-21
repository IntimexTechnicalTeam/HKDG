namespace HKDG.BLL
{
    public class UpdateQtyWhenAddToCartService : AbstractCalculateQtyService
    {
        IServiceProvider _service;
        IRabbitMQService RabbitMQService { get; set; }

        public UpdateQtyWhenAddToCartService(IServiceProvider services) : base(services)
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

            //设置Hold货数量
            var InvtHoldQty = await RedisHelper.ZIncrByAsync(InvtHoldQtyKey, SkuId.ToString(), Qty);

            //设置可销售库存数
            var SalesQty = InvtActualQty - InvtReservedQty - InvtHoldQty;
            if (SalesQty < 0) SalesQty = 0;
            await RedisHelper.ZAddAsync(SalesQtyKey, (SalesQty, SkuId));

            var result = await base.SendCalculateQty(InvtActualQty, InvtHoldQty, SalesQty, InvtReservedQty, SkuId, MsgId, QtyType.WhenAddToCart);
            return result;
        }
    }
}
