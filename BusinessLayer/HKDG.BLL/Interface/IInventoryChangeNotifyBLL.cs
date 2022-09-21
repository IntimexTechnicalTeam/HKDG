namespace HKDG.BLL
{
    public interface IInventoryChangeNotifyBLL:IDependency
    {
        SystemResult AddInventoryChangeNotify(InventoryChangeNotify notify);

        Task CheckAndNotifyAsync(IList<Guid> skuIds);
    }
}
