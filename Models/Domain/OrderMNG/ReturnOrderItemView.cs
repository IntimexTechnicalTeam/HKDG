using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ReturnOrderItemView
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid SkuId { get; set; }
        public Guid MerchantId { get; set; }
        public string TrackingNo { get; set; }
        public decimal UnitPrice { get; set; }
        public int Qty { get; set; }
        public Guid DeliveryId { get; set; }
        public string DeliveryNo { get; set; }
        public ProductSummary Product { get; set; }
        public ProductSku SkuInfo { get; set; }
        /// <summary>
        /// 退貨單附加圖片
        /// </summary>
        public List<ReturnOrderImageDto> AttachImages { get; set; } = new List<ReturnOrderImageDto>();
        public List<ReturnOrderImageView> AttachImagePaths { get; set; } = new List<ReturnOrderImageView>();
        public string Costctr { get; set; }
        public decimal NetRcd { get; set; }
        public string SupplierNum { get; set; }
        public string AccountCode { get; set; }
    }
}
