
using System.ComponentModel;

namespace Domain
{
    public class ArrivalNotifyDto
    {
      
        public Guid Id { get; set; }
        
        public Guid ClientId { get; set; }


        /// <summary>
        /// 會員ID
        /// </summary>

        public Guid? MemberId { get; set; }
        /// <summary>
        /// Sku
        /// </summary>

        public Guid SkuId { get; set; }
        /// <summary>
        /// 是否已通知會員
        /// </summary>
     
        public bool IsNotified { get; set; }
        /// <summary>
        /// 通知會員時間
        /// </summary>
       
        public DateTime? NotifyDate { get; set; }
        /// <summary>
        /// 通知郵箱
        /// </summary>
        
        public string Email { get; set; }
        /// <summary>
        /// 是否已通知商家
        /// </summary>
      
        public bool IsMerchNotified { get; set; }
        /// <summary>
        /// 通知商家時間
        /// </summary>
       
        public DateTime? MerchNotifyDate { get; set; }
        /// <summary>
        /// 商家Id
        /// </summary>

        public Guid MerchantId { get; set; }


        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreateDate { get; set; }
       
        public DateTime? UpdateDate { get; set; }

        public Guid CreateBy { get; set; }

        public Guid? UpdateBy { get; set; }
    }
}
