using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class PromotionCond
    {
        public PromotionCond()
        {
          
        }

        public ClientSideType From { get; set; } = ClientSideType.Mobile;


        public string Name { get; set; }

        public string Desc { get; set; }

        //public Language? Language { get; set; }

        //public bool? CondIsActive { get; set; }

        public string PageStr { get; set; }

        public int Position { get; set; } = -1;

        public bool ShowBanner { get; set; }
        public bool ShowProduct { get; set; }
        public bool ShowMerchant { get; set; }

        /// <summary>
        /// 推廣產品獲取數量
        /// </summary>
        public int ProductSize { get; set; } = 0;

        /// <summary>
        /// 推廣商家獲取數量
        /// </summary>
        public int MerchantSize { get; set; } = 0;

        /// <summary>
        /// 推廣商家下產品的獲取數量
        /// </summary>
        public int MerchantProductSize { get; set; } = 0;

        public int PrmtType { get; set; }

        //public Guid PrmtDetailId { get; set; }

        public PageInfo PageInfo { get; set; } = new PageInfo();

        public string IspType { get; set; }
    }
}
