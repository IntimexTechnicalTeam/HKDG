using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.MQ
{
    public static class MQSetting
    {     
		 /// <summary>
        /// 更新ProductQty队列
        /// </summary>
        public const string UpdateQtyQueue = "Update.Qty.Queue";
        public const string UpdateQtyExchange = "Update.Qty.Exchange";
     
        ///// <summary>
        ///// 支付超时队列
        ///// </summary>
        //public const string WeChatPayTimeOutQueue = "HKDG.PayTimeOut.Queue";
        //public const string WeChatPayTimeOutExchange = "HKDG.PayTimeOut.Exchange";
        //public const string DelayPayTimeOutQueue = "Delay.PayTimeOut.Queue";


        public const string ProxyCommissionQueue = "Proxy.Commission.Queue";
        public const string ProxyCommissionExchange = "Proxy.Commission.Exchange";

        /// <summary>
        /// 更新库存队列
        /// </summary>
        public const string CalculateQtyQueue = "Calculate.Qty.Queue";

        /// <summary>
        /// 
        /// </summary>
        public const string CalculateQtyExchange = "Calculate.Qty.Exchange";

    }
}
