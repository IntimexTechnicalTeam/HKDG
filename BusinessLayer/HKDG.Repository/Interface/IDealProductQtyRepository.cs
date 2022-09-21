namespace HKDG.Repository
{
    public interface IDealProductQtyRepository : IDependency
    {
        Task<SystemResult> UpdateQty(int InvtActualQty, int InvtHoldQty, int SalesQty, int InvtReservedQty, Guid SkuId, Guid MsgId);

        /// <summary>
        /// 安排发货后，更新 InvtReservedQty，InvtActualQty，SalesQty
        /// </summary>
        /// <param name="InvtActualQty"></param>
        /// <param name="SalesQty"></param>
        /// <param name="InvtReservedQty"></param>
        /// <param name="SkuId"></param>
        /// <param name="MsgId"></param>
        /// <returns></returns>
        Task<SystemResult> UpdateQty(int InvtActualQty, int SalesQty, int InvtReservedQty, Guid SkuId, Guid MsgId);

        /// <summary>
        /// 创建订单,支付前取消，支付后到發貨前取消，支付超時或失败时，更新 InvtReservedQty
        /// </summary>
        /// <param name="InvtReservedQty"></param>
        /// <param name="SkuId"></param>
        /// <param name="MsgId"></param>
        /// <returns></returns>
        Task<SystemResult> UpdateQty(int InvtReservedQty, Guid SkuId, Guid MsgId);

        /// <summary>
        /// 采購入庫,采購退回,銷售退回，更新 InvtActualQty，SalesQty
        /// </summary>
        /// <param name="InvtActualQty"></param>
        /// <param name="SalesQty"></param>
        /// <param name="SkuId"></param>
        /// <param name="MsgId"></param>
        /// <returns></returns>
        Task<SystemResult> UpdateQty(int InvtActualQty, int SalesQty, Guid SkuId, Guid MsgId);

    }
}
