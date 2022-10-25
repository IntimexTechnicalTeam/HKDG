namespace Domain
{
    public class HotMerchantFreeCharge
    {
        public Guid Id { get; set; }

        public Guid MchId { get; set; }

        public string ProductCode { get; set; }

        public string ShipCode { get; set; }
    }
}
