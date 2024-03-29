﻿namespace HKDG.Repository
{
    public interface IOrderRepository:IDependency
    {
        PageData<OrderView> GetSimpleOrderByPage(OrderCondition cond);

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="order"></param>
        void UpdateOrderStatus(Order order);

        PageData<OrderSummaryView> GetOrderByPage(OrderCondition cond);
    }
}
