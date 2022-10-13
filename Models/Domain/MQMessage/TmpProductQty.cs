namespace Domain
{
    public class TmpProductQty
    {
        /// <summary>
        /// PushMessage.Id
        /// </summary>
        public Guid Id { get; set; }

        public Guid SkuId { get; set; }

        public int InvtActualQty { get; set; }

        public int SalesQty { get; set; }

        public int InvtReservedQty { get; set; }

        public int InvtHoldQty { get; set; }

        public QtyType QtyType { get; set; }

    }

    public class CalculateQty
    {

        public Guid Id { get; set; }

        public Guid SkuId { get; set; }

        public int Qty { get; set; }

        public QtyType QtyType { get; set; }

        public Guid RelatedId { get; set; } = Guid.Empty;
    }
}
