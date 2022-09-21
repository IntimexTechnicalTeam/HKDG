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
        public const string UpdateQtyQueue = "HKDG.Update.Qty.Queue";
        public const string UpdateQtyExchange = "HKDG.Update.Qty.Exchange";
     
        /// <summary>
        /// 支付超时队列
        /// </summary>
        public const string WeChatPayTimeOutQueue = "HKDG.PayTimeOut.Queue";
        public const string WeChatPayTimeOutExchange = "HKDG.PayTimeOut.Exchange";
        public const string DelayPayTimeOutQueue = "Delay.PayTimeOut.Queue";


        public const string ProxyCommissionQueue = "HKDG.Proxy.Commission.Queue";
        public const string ProxyCommissionExchange = "HKDG.Proxy.Commission.Exchange";

        /// <summary>
        /// 更新库存队列
        /// </summary>
        public const string CalculateQtyQueue = "HKDG.Calculate.Qty.Queue";

        /// <summary>
        /// 
        /// </summary>
        public const string CalculateQtyExchange = "HKDG.Calculate.Qty.Exchange";

    }
}
