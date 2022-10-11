namespace HKDG.Enums
{
    /// <summary>
    /// 购物车中数据的状态
    /// </summary>
    public enum CartItemStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,

        /// <summary>
        /// 已下架或已删除
        /// </summary>
        OffSale,

        /// <summary>
        /// 库存紧张
        /// </summary>
        StressOfStock,

        /// <summary>
        /// 库存不足
        /// </summary>
        OutOfStock,
    }
}
