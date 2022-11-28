namespace Model
{
    public class ProductAttrImgs
    {
        public Guid Id { get; set; }

        public string ProductCode { get; set; }

        public Guid ProductId { get; set; }

        public Guid AttrValue1 { get; set; }

        public Guid AttrValue2 { get; set; }

        public Guid AttrValue3 { get; set; }



        /// <summary>
        /// 是否库存属性
        /// </summary>
        public bool IsInventory { get; set; } = false;

        /// <summary>
        /// 圖片的類型,屬性圖片、附加圖片
        /// </summary>
        public ImageType Type { get; set; }

        /// <summary>
        /// 图片的正反面
        /// </summary>

        public ImageSide Side { get; set; }

        /// <summary>
        /// 是否默认图片
        /// </summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// 图片集合
        /// </summary>
        public List<string> ImageItems { get; set; } = new List<string>();

    }
}
