namespace HKDG.BLL
{
    /// <summary>
    /// 完成发货后，进行銷售退回或發貨退回，相当于重新采购入库
    /// </summary>
    public class UpdateQtyWhenReturnService : AbstractCalculateQtyService
    {
        IServiceProvider _service;

        public UpdateQtyWhenReturnService(IServiceProvider services) : base(services)
        {
            this._service = services;

        }

        public override async Task<SystemResult> CalcuteQty(Guid SkuId, int Qty, Guid MsgId)
        {
            string SalesQtyKey = $"{CacheKey.SalesQty}";          
            string InvtActualQtyKey = $"{CacheKey.InvtActualQty}";
           
            //设置实际库存数
            var InvtActualQty = await RedisHelper.ZIncrByAsync(InvtActualQtyKey, SkuId.ToString(), Qty);
            //设置可销售库存数           
            var SalesQty = await RedisHelper.ZIncrByAsync(SalesQtyKey, SkuId.ToString(), Qty);

            //更新MsgId 的CalculateQtyQueue表状态
            var result = await base.SendCalculateQty((int)InvtActualQty,(int) SalesQty, SkuId, MsgId, QtyType.WhenReturn);
            return result;
        }
    }
}
