using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TransinMbrDetail
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNum { get; set; }

        /// <summary>
        /// TranSin的T-Points总数
        /// </summary>
        public decimal TPoint { get; set; }

        /// <summary>
        /// TranSin的T-Marks总数
        /// </summary>
        public decimal TMark { get; set; }

        /// <summary>
        /// 兑换前的T-Points数值
        /// </summary>
        public decimal BeforeFun { get; set; }

        /// <summary>
        /// 实际可用的B-Points数值
        /// </summary>
        public decimal RealFun { get; set; }

        /// <summary>
        /// 当天可兑换的T-Points额度
        /// </summary>
        public decimal CurrentDayTPoints { get; set; }

        /// <summary>
        /// 当月可兑换的T-Points额度
        /// </summary>
        public decimal CurrnetMonthTPoints { get; set; }

        /// <summary>
        /// 当年可兑换的T-Points额度
        /// </summary>
        public decimal CurrnetYearTPoints { get; set; }

        /// <summary>
        /// 每天最大兑换积分数
        /// </summary>
        public decimal MaxLimitDayFun { get; set; }

        /// <summary>
        /// 月度最大兑换积分数
        /// </summary>
        public decimal MaxLimitMonthFun { get; set; }

        /// <summary>
        /// 年度最大兑换积分数
        /// </summary>
        public decimal MaxLimitYearFun { get; set; }

        /// <summary>
        /// 比例值
        /// </summary>
        public string ScaleValue { get; set; }
    }
}
