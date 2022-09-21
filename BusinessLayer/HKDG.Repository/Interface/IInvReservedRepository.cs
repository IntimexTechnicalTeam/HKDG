namespace HKDG.Repository
{
    public interface IInvReservedRepository:IDependency
    {
        List<InventoryReserved> GetInvReservedLst(InventoryReserved cond);

        void DelInvReserved(InventoryReserved cond);
    }
}
