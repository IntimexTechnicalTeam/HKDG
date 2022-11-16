namespace Domain
{
    public class HotPromotionBanner
    {
        public Guid Id { get; set; }

        public Guid PrmtID { get; set; }

        public Guid PrmtDtlId { get; set; }

        public string ImgPath { get; set; }

        public string Image { get; set; }

        public int ImgW { get; set; } = 0;

        public int ImgH { get; set; } = 0;

        public string Url { get; set; }
        public int Seq { get; set; }

        public string Content { get; set; }
    }
}
