namespace Domain
{
    public class HotPromotionProduct
    {
        public Guid Id { get; set; }

        public Guid PromotionId { get; set; }

        public string ProdCode { get; set; }

        public Guid ProductId { get; set; }

        public int Seq { get; set; }

        public Guid PrmtDtlId { get; set; }

        public Guid MchId { get; set; }
    }
}
