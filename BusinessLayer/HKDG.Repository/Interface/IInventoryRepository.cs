namespace HKDG.Repository
{
    public interface IInventoryRepository:IDependency
    {
        List<Inventory> GetInventoryList(InventoryDto cond);

        PageData<InvSummary> GetInvSummaryByPage(InvSrchCond cond);
    }
}
