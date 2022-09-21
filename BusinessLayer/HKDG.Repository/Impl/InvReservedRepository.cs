namespace HKDG.Repository
{
    public class InvReservedRepository : PublicBaseRepository, IInvReservedRepository
    {
        public InvReservedRepository(IServiceProvider service) : base(service)
        {
        }

        public List<InventoryReserved> GetInvReservedLst(InventoryReserved cond)
        {
            List<InventoryReserved> invReservedLst = new List<InventoryReserved>();

            var invReservedQuery = baseRepository.GetList<InventoryReserved>(x =>x.IsActive && !x.IsDeleted  && x.OrderId != Guid.Empty);

            if (cond != null)
            {
                if (cond.Sku != Guid.Empty)
                {
                    invReservedQuery = invReservedQuery.Where(x => x.Sku == cond.Sku);
                }
                if (cond.OrderId != Guid.Empty)
                {
                    invReservedQuery = invReservedQuery.Where(x => x.OrderId == cond.OrderId);
                }
                if (cond.SubOrderId != Guid.Empty)
                {
                    invReservedQuery = invReservedQuery.Where(x => x.SubOrderId == cond.SubOrderId);
                }
                if (cond.ReservedType > 0)
                {
                    invReservedQuery = invReservedQuery.Where(x => x.ReservedType == cond.ReservedType);
                }
                if (cond.ProcessState > 0)
                {
                    invReservedQuery = invReservedQuery.Where(x => x.ProcessState == cond.ProcessState);
                }
            }
            invReservedLst = invReservedQuery.ToList();

            return invReservedLst;
        }

        public void DelInvReserved(InventoryReserved cond)
        {
            string sql = @"delete from  InventoryReserveds  where OrderId=@OrderId and SubOrderId=@SubOrderId  and sku=@sku and ProcessState=@ProcessState";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@OrderId", cond.OrderId));
            paramList.Add(new SqlParameter("@SubOrderId", cond.SubOrderId));
            paramList.Add(new SqlParameter("@sku", cond.Sku));
            paramList.Add(new SqlParameter("@ProcessState", cond.ProcessState));

            var result = baseRepository.ExecuteSqlCommand(sql, paramList.ToArray());

            //return invReservedLst;
        }
    }
}
