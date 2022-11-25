using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ProductCommentDto
    {
        /// <summary>
        /// 商家ID
        /// </summary>

        public Guid MerchantId { get; set; }
        /// <summary>
        /// 訂單ID
        /// </summary>

        public Guid OrderId { get; set; }


        public Guid SubOrderId { get; set; }
        /// <summary>
        /// 產品Id
        /// </summary>

        public Guid ProductId { get; set; }
        /// <summary>
        /// 評分
        /// </summary>

        public decimal Score { get; set; }
        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }
        ///// <summary>
        ///// 回復人ID
        ///// </summary>
        //public Guid ReplyId { get; set; }

        /// <summary>
        /// 回復內容
        /// </summary>
        public string ReplyContent { get; set; }
        /// <summary>
        /// 是否顯示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 评论图片
        /// </summary>
        public List<ProductCommentImageDto> CommentImages { get; set; } = new List<ProductCommentImageDto>();
        /// <summary>
        /// 產品編號
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 產品图片
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 產品名稱
        /// </summary>
      
        public string ProductImg { get; set; }
        /// <summary>
        /// 訂單編號
        /// </summary>

        public string OrderNO { get; set; }
        /// <summary>
        /// 發貨單編號
        /// </summary>

        public string DeliveryNo { get; set; }


        public string MemberName { get; set; }

        public string MerchantName { get; set; }

        public Guid Id { get; set; }
 
        public Guid ClientId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }


        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }
    }
}
