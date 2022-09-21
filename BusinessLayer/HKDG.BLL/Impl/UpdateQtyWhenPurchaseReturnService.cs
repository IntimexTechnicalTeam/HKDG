namespace HKDG.BLL
{
    public class UpdateQtyWhenPurchaseReturnService : AbstractCalculateQtyService
    {
        IServiceProvider _service;

        public UpdateQtyWhenPurchaseReturnService(IServiceProvider services) : base(services)
        {
            this._service = services;

        }

        public override async Task<SystemResult> CalcuteQty(Guid SkuId, int Qty, Guid MsgId)
        {
            string SalesQtyKey = $"{CacheKey.SalesQty}";
            string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";

            //设置实际库存数       
            var InvtActualQty = await RedisHelper.ZIncrByAsync(InvtActualQtyKey, SkuId.ToString(), -Qty);
            //设置可销售库存数
            var SalesQty = await RedisHelper.ZIncrByAsync(SalesQtyKey, SkuId.ToString(), -Qty);

            var result = await base.SendCalculateQty((int)InvtActualQty,(int)SalesQty,SkuId, MsgId, QtyType.WhenPurchasingReturn);
            return result;
        }
    }

}
