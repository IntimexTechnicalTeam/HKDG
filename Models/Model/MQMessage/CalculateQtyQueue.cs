
namespace Model
{
    public class CalculateQtyQueue : BaseEntity<Guid>
    {
        public Guid SkuId { get; set; }

        public int Qty { get; set; }

        public QtyType QtyType { get; set; }

        /// <summary>
        /// ShoppingCartItem.Id or OrderId
        /// </summary>
        public Guid RelatedId { get; set; } = Guid.Empty;

        public MQState State { get; set; } = MQState.UnDeal;
    }
}
