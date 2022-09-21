namespace HKDG.BLL
{
    public interface IDealProductQtyCacheBLL : IDependency
    {
        /// <summary>
        /// 当订单状态改变时，更新库存
        /// </summary>
        /// <param name="orderStatusInfo"></param>
        /// <returns></returns>
        Task<SystemResult> UpdateQtyWhenOrderStateChange(UpdateStatusCondition orderStatusInfo);

        /// <summary>
        /// 采购退回，銷售退回，發貨退回时更新库存
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<SystemResult> UpdateQtyWhenPurchaseOrReturn(List<InvTransactionDtlDto> list);

        /// <summary>
        /// 支付超时，回滚
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        Task<SystemResult> UpdateQtyWhenPayTimeOut(Guid OrderId);
    }
}