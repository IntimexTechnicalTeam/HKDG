namespace Domain
{
    public class SalesReturnOrderDto:BaseDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 單號
        /// </summary>
        public string OrderNo { get; set; } = "";
        /// <summary>
        /// 銷售單ID
        /// </summary>
      
        public Guid SOId { get; set; }

        /// <summary>
        /// 銷售退回時間
        /// </summary>

        public DateTime ReturnDate { get; set; }

        public List<SalesReturnOrderDetailDto> SalesReturnItemList { get; set; } = new List<SalesReturnOrderDetailDto>();

        public virtual void Validate()
        {          
            if (SalesReturnItemList == null || !SalesReturnItemList.Any()) throw new BLException(Message.OneSelect);
            foreach (var item in SalesReturnItemList)
            {
                if (item.ReturnQty < 0) throw new BLException(Message.SalesReturnQtySufficient);
                if (item.ReturnQty > item.OrderQty)  throw new BLException(Message.SalesReturnQtyOverrun);
            }
        }
    }
}
