namespace HKDG.BLL
{
    public class UpdateQtyWhenCreateOrderService : AbstractCalculateQtyService
    {
        IServiceProvider _service;
        IRabbitMQService RabbitMQService { get; set; }
        public UpdateQtyWhenCreateOrderService(IServiceProvider services) : base(services)
        {
            this._service = services;
        }

        public override async Task<SystemResult> CalcuteQty(Guid SkuId, int Qty, Guid MsgId)
        {           
            //冻结Qty
            string InvtReservedQtyKey = $"{CacheKey.InvtReservedQty}";
            var InvtReservedQty = await RedisHelper.ZIncrByAsync(InvtReservedQtyKey, SkuId.ToString(), Qty);
           
            var result = await base.SendCalculateQty((int)InvtReservedQty, SkuId, MsgId, QtyType.WhenCreateOrder);
            return result;
        }
    }
}
