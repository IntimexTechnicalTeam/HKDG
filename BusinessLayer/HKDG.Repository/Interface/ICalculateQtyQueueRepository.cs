namespace HKDG.Repository
{
    public interface ICalculateQtyQueueRepository : IDependency
    {
        Task<int> UpdateState(Guid Id);
    }
}
