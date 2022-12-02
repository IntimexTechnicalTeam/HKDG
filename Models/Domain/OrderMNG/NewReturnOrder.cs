using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class NewReturnOrder
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public List<ReturnOrderItemView> Items { get; set; } = new List<ReturnOrderItemView>();
        public ReturnOrderType ApplyType { get; set; }
        public string Message { get; set; }
        /// <summary>
        /// 是否全新創建
        /// </summary>
        /// <remarks>如果檢查到相關退換單已創建，則設置為false</remarks>
        public bool isNew { get; set; }
    }
}
