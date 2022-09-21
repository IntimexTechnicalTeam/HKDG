using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKDG.Enums
{
    public enum QtyType
    {

        WhenPurchasing,
        /// <summary>
        /// 采购退回
        /// </summary>
        WhenPurchasingReturn,
        /// <summary>
        /// 销售退回，發貨退回
        /// </summary>
        WhenReturn,
        WhenAddToCart,
        WhenDeleteCart,
        WhenModifyCart,
        WhenPay,
        WhenDeliveryArranged,
        WhenOrderCancel,
        /// <summary>
        /// 支付超时，回滚Hold货数量
        /// </summary>
        WhenPayTimeOut,

        WhenCreateOrder,
    }
}
