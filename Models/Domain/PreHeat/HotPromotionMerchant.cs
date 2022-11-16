namespace Domain
{
    public class HotPromotionMerchant
    {
        public Guid Id { get; set; }

        public Guid PrmtID { get; set; }
       
        public Guid MerchantId { get; set; }

        public int Seq { get; set; }

        public Guid PrmtDtlId { get; set; }
    }
}
