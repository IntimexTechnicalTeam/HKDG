using System.ComponentModel;

namespace Domain
{
    public class ReturnOrderImageDto :BaseDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 退換單詳細ID
        /// </summary>

        public Guid ROrderDtlId { get; set; }
       
        /// <summary>
        /// 退換單ID
        /// </summary>
        public Guid ROrderId { get; set; }
        /// <summary>
        /// 縮略圖片路徑
        /// </summary>

        public string ImageS { get; set; }
        /// <summary>
        /// 原尺寸圖片路徑
        /// </summary>

        public string ImageB { get; set; }
        /// <summary>
        /// 縮略圖圖片名稱
        /// </summary>
        public string ImageSName { get; set; }
        /// <summary>
        /// 縮略圖圖片新名稱
        /// </summary>
        public string NewImageSName { get; set; }
        /// <summary>
        /// 原圖圖片名稱
        /// </summary>
        public string ImageBName { get; set; }
        /// <summary>
        /// 原圖圖片新名稱
        /// </summary>
        public string NewImageBName { get; set; }
        /// <summary>
        /// 圖片類別
        /// </summary>
        public ReturnOrderImgType Type { get; set; }
        /// <summary>
        /// 付運日期
        /// </summary>
        public DateTime? DeliveryDate { get; set; }
    }
}
