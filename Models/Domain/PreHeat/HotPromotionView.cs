namespace Domain
{
    public class HotPromotionView
    {
        /// <summary>
        /// Promotion.Id
        /// </summary>
        public Guid Id { get; set; }

        public Guid PromotionDetailsId { get; set; }

        public ClientSideType ClientType { get; set; }

        public string Name { get; set; }

        public int Seq { get; set; }

        public string Page { get; set; }

        public Language LangType { get; set; }

        public string Desc { get; set; }

        public string ImgPath { get; set; }

        public PrmtLayoutType PrmtLType { get; set; }

        public int Type => (int)PrmtLType;

        public string IspType { get; set; }
    }
}
