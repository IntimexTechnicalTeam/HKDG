using Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace HKDG.BLL
{
    public class UpdateQtyWhenPurchasingService : AbstractCalculateQtyService
    {
        IServiceProvider _service;

        public UpdateQtyWhenPurchasingService(IServiceProvider services) : base(services)
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
            var result = await base.SendCalculateQty((int)InvtActualQty, (int)SalesQty, SkuId, MsgId, QtyType.WhenPurchasing);
            return result;
        }
    }
}
